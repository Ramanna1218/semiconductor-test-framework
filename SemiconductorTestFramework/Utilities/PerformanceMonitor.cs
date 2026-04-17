using System;
using System.Diagnostics;

namespace SemiconductorTestFramework.Utilities
{
    public class PerformanceMonitor
    {
        private Stopwatch _stopwatch;
        private long _startMemory;

        public void StartMonitoring()
        {
            GC.Collect();
            _startMemory = GC.GetTotalMemory(false);
            _stopwatch = Stopwatch.StartNew();
        }

        public void StopMonitoring()
        {
            _stopwatch.Stop();
        }

        public long GetElapsedMs() => _stopwatch.ElapsedMilliseconds;

        public long GetMemoryUsedMB()
        {
            long endMemory = GC.GetTotalMemory(false);
            return (endMemory - _startMemory) / (1024 * 1024);
        }

        public void PrintPerformanceMetrics()
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("PERFORMANCE METRICS");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"Total Execution Time: {GetElapsedMs()}ms");
            Console.WriteLine($"Memory Used: {GetMemoryUsedMB()}MB");
            Console.WriteLine($"Processor Count: {Environment.ProcessorCount}");
            Console.WriteLine(new string('=', 70) + "\n");
        }
    }
}