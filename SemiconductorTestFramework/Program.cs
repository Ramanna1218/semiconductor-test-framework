using System;
using System.Collections.Generic;
using SemiconductorTestFramework.Core;
using SemiconductorTestFramework.TestSuites;
using SemiconductorTestFramework.Utilities;

namespace SemiconductorTestFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║     Automated Test Framework for Simulated Semiconductor Equipment  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝\n");

            var logger = new Logger("Output/logs", LogLevel.Debug);
            var resultTracker = new ResultTracker("Output/results");
            var performanceMonitor = new PerformanceMonitor();

            logger.LogInfo("Test framework initialized");
            logger.LogInfo($"Log file: {logger.GetLogFilePath()}");

            var executor = new TestExecutor(logger, resultTracker);

            Console.WriteLine("Loading test suites...\n");

            var temperatureTests = TemperatureTests.GetTemperatureTestSuite();
            var voltageTests = VoltageTests.GetVoltageTestSuite();
            var signalTests = SignalIntegrityTests.GetSignalIntegrityTestSuite();
            var powerTests = PowerTests.GetPowerTestSuite();
            var systemTests = SystemTests.GetSystemTestSuite();

            executor.RegisterMultipleTestCases(temperatureTests);
            executor.RegisterMultipleTestCases(voltageTests);
            executor.RegisterMultipleTestCases(signalTests);
            executor.RegisterMultipleTestCases(powerTests);
            executor.RegisterMultipleTestCases(systemTests);

            Console.WriteLine($"Total tests loaded: {executor.GetTestCount()}\n");
            Console.WriteLine("Test Breakdown:");
            Console.WriteLine($"  - Temperature Tests: {temperatureTests.Count}");
            Console.WriteLine($"  - Voltage Tests: {voltageTests.Count}");
            Console.WriteLine($"  - Signal Integrity Tests: {signalTests.Count}");
            Console.WriteLine($"  - Power Tests: {powerTests.Count}");
            Console.WriteLine($"  - System Tests: {systemTests.Count}\n");

            int threadCount = Environment.ProcessorCount;
            executor.SetMaxThreads(threadCount);
            Console.WriteLine($"Multi-threaded execution enabled with {threadCount} threads\n");

            performanceMonitor.StartMonitoring();
            Console.WriteLine("Starting test execution...\n");

            var results = executor.ExecuteAllTests(parallel: true);

            performanceMonitor.StopMonitoring();

            executor.PrintSummary();
            performanceMonitor.PrintPerformanceMetrics();

            logger.LogInfo($"Test results saved to: {resultTracker.GetResultsDirectory()}");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}