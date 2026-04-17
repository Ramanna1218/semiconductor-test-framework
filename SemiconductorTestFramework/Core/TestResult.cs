using System;

namespace SemiconductorTestFramework.Core
{
    public enum TestStatus
    {
        NotRun,
        Running,
        Passed,
        Failed,
        Skipped,
        Timeout
    }

    public class TestResult
    {
        public string TestId { get; set; }
        public string TestName { get; set; }
        public TestStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long DurationMs { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public TestResult(string testId, string testName)
        {
            TestId = testId;
            TestName = testName;
            Status = TestStatus.NotRun;
        }

        public override string ToString()
        {
            return $"[{Status}] {TestName} - {DurationMs}ms";
        }
    }
}