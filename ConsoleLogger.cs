using System;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}");
            Console.ResetColor();
        }
    }
}

