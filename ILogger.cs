using System;

namespace XmlToPdfConverter.Core.Interfaces
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public interface ILogger
    {
        void Log(string message, LogLevel level = LogLevel.Info);
    }

    public static class LoggerExtensions
    {
        public static void LogDebug(this ILogger logger, string message)
        {
            logger?.Log(message, LogLevel.Debug);
        }

        public static void LogInfo(this ILogger logger, string message)
        {
            logger?.Log(message, LogLevel.Info);
        }

        public static void LogWarning(this ILogger logger, string message)
        {
            logger?.Log(message, LogLevel.Warning);
        }

        public static void LogError(this ILogger logger, string message)
        {
            logger?.Log(message, LogLevel.Error);
        }

        public static void LogError(this ILogger logger, string message, Exception ex)
        {
            logger?.Log($"{message} - Exception: {ex.Message}", LogLevel.Error);
        }
    }
}

