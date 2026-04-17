using System;

namespace SemiconductorTestFramework.Core
{
    public class TestCase
    {
        public string TestId { get; set; }
        public string TestName { get; set; }
        public string Category { get; set; }
        public Action<HardwareSimulator> TestExecutionLogic { get; set; }
        public int TimeoutMs { get; set; } = 5000;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public TestCase(string testId, string testName, string category)
        {
            TestId = testId;
            TestName = testName;
            Category = category;
        }
    }
}