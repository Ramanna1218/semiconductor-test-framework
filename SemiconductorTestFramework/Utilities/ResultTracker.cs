using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SemiconductorTestFramework.Core;

namespace SemiconductorTestFramework.Utilities
{
    public class ResultTracker
    {
        private string _resultsDirectory;

        public ResultTracker(string resultsDirectory = "results")
        {
            _resultsDirectory = resultsDirectory;
            if (!Directory.Exists(_resultsDirectory))
                Directory.CreateDirectory(_resultsDirectory);
        }

        public void SaveResults(List<TestResult> results)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            string filePath = Path.Combine(_resultsDirectory, $"results_{timestamp}.json");

            var serializedResults = results.Select(r => new
            {
                r.TestId,
                r.TestName,
                Status = r.Status.ToString(),
                r.StartTime,
                r.EndTime,
                r.DurationMs,
                r.ErrorMessage
            }).ToList();

            string json = JsonConvert.SerializeObject(serializedResults, Formatting.Indented);
            File.WriteAllText(filePath, json);

            SaveResultsAsCSV(results, timestamp);
        }

        private void SaveResultsAsCSV(List<TestResult> results, string timestamp)
        {
            string csvPath = Path.Combine(_resultsDirectory, $"results_{timestamp}.csv");
            using (var writer = new StreamWriter(csvPath))
            {
                writer.WriteLine("TestId,TestName,Status,StartTime,EndTime,DurationMs,ErrorMessage");
                foreach (var result in results)
                {
                    string errorMsg = result.ErrorMessage?.Replace(",", ";") ?? "";
                    writer.WriteLine($"{result.TestId},{result.TestName},{result.Status}," +
                        $"{result.StartTime:yyyy-MM-dd HH:mm:ss.fff}," +
                        $"{result.EndTime:yyyy-MM-dd HH:mm:ss.fff}," +
                        $"{result.DurationMs},{errorMsg}");
                }
            }
        }

        public string GetResultsDirectory() => _resultsDirectory;
    }
}