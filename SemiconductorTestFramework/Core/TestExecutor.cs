using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SemiconductorTestFramework.Utilities;

namespace SemiconductorTestFramework.Core
{
    public class TestExecutor
    {
        private List<TestCase> _testCases;
        private List<TestResult> _testResults;
        private Logger _logger;
        private ResultTracker _resultTracker;
        private HardwareSimulator _simulator;
        private int _maxThreads = Environment.ProcessorCount;

        public TestExecutor(Logger logger, ResultTracker resultTracker)
        {
            _logger = logger;
            _resultTracker = resultTracker;
            _testCases = new List<TestCase>();
            _testResults = new List<TestResult>();
            _simulator = new HardwareSimulator();
            _simulator.Initialize();
        }

        public void RegisterTestCase(TestCase testCase)
        {
            _testCases.Add(testCase);
            _logger.LogInfo($"Test registered: {testCase.TestName}");
        }

        public void RegisterMultipleTestCases(List<TestCase> testCases)
        {
            _testCases.AddRange(testCases);
            _logger.LogInfo($"{testCases.Count} tests registered");
        }

        public List<TestResult> ExecuteAllTests(bool parallel = true)
        {
            _logger.LogInfo($"Starting test execution - Total tests: {_testCases.Count}");
            var stopwatch = Stopwatch.StartNew();

            if (parallel)
                ExecuteTestsParallel();
            else
                ExecuteTestsSequential();

            stopwatch.Stop();
            _logger.LogInfo($"Test execution completed in {stopwatch.ElapsedMilliseconds}ms");
            _resultTracker.SaveResults(_testResults);

            return _testResults;
        }

        private void ExecuteTestsSequential()
        {
            foreach (var testCase in _testCases)
            {
                ExecuteSingleTest(testCase);
            }
        }

        private void ExecuteTestsParallel()
        {
            Parallel.ForEach(_testCases, new ParallelOptions { MaxDegreeOfParallelism = _maxThreads }, testCase =>
            {
                ExecuteSingleTest(testCase);
            });
        }

        private void ExecuteSingleTest(TestCase testCase)
        {
            var result = new TestResult(testCase.TestId, testCase.TestName);
            result.StartTime = DateTime.Now;
            result.Status = TestStatus.Running;

            try
            {
                _logger.LogDebug($"[START] {testCase.TestName}");

                var task = Task.Run(() =>
                {
                    testCase.TestExecutionLogic(_simulator);
                });

                if (!task.Wait(testCase.TimeoutMs))
                {
                    result.Status = TestStatus.Timeout;
                    result.ErrorMessage = $"Test exceeded timeout of {testCase.TimeoutMs}ms";
                    _logger.LogError($"[TIMEOUT] {testCase.TestName}");
                }
                else
                {
                    result.Status = TestStatus.Passed;
                    _logger.LogInfo($"[PASS] {testCase.TestName}");
                }
            }
            catch (Exception ex)
            {
                result.Status = TestStatus.Failed;
                result.ErrorMessage = ex.Message;
                result.StackTrace = ex.StackTrace;
                _logger.LogError($"[FAIL] {testCase.TestName}: {ex.Message}");
            }
            finally
            {
                result.EndTime = DateTime.Now;
                result.DurationMs = (long)(result.EndTime - result.StartTime).TotalMilliseconds;
                _testResults.Add(result);
                _simulator.ResetState();
            }
        }

        public void PrintSummary()
        {
            int passed = 0, failed = 0, timeout = 0;
            long totalTime = 0;

            foreach (var result in _testResults)
            {
                totalTime += result.DurationMs;
                if (result.Status == TestStatus.Passed) passed++;
                else if (result.Status == TestStatus.Failed) failed++;
                else if (result.Status == TestStatus.Timeout) timeout++;
            }

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("TEST EXECUTION SUMMARY");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"Total Tests: {_testResults.Count}");
            Console.WriteLine($"Passed: {passed}");
            Console.WriteLine($"Failed: {failed}");
            Console.WriteLine($"Timeout: {timeout}");
            Console.WriteLine($"Success Rate: {(passed * 100.0 / _testResults.Count):F1}%");
            Console.WriteLine($"Total Execution Time: {totalTime}ms");
            Console.WriteLine(new string('=', 70) + "\n");
        }

        public void SetMaxThreads(int threads)
        {
            _maxThreads = Math.Max(1, Math.Min(threads, Environment.ProcessorCount));
        }

        public int GetTestCount() => _testCases.Count;
        public List<TestResult> GetResults() => _testResults;
    }
}