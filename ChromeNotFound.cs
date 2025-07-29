using System;

namespace XmlToPdfConverter.Core.Configuration
{
    public class ChromeNotFoundException : Exception
    {
        public ChromeNotFoundException(string message) : base(message) { }
        public ChromeNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}