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
}

