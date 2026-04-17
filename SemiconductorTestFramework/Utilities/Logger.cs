using System;
using System.IO;

namespace SemiconductorTestFramework.Utilities
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public class Logger
    {
        private string _logFilePath;
        private LogLevel _minLogLevel;
        private readonly object _lockObject = new object();

        public Logger(string logDirectory = "logs", LogLevel minLevel = LogLevel.Debug)
        {
            _minLogLevel = minLevel;
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            _logFilePath = Path.Combine(logDirectory, $"test_log_{timestamp}.txt");
        }

        public void LogDebug(string message) => Log(LogLevel.Debug, message);
        public void LogInfo(string message) => Log(LogLevel.Info, message);
        public void LogWarning(string message) => Log(LogLevel.Warning, message);
        public void LogError(string message) => Log(LogLevel.Error, message);

        private void Log(LogLevel level, string message)
        {
            if (level < _minLogLevel) return;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logMessage = $"[{timestamp}] [{level}] {message}";

            lock (_lockObject)
            {
                Console.WriteLine(logMessage);
                try
                {
                    File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                }
            }
        }

        public string GetLogFilePath() => _logFilePath;
    }
}