using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Progress
{
    public class SilentProgressReporter : IProgressReporter
    {
        public void Report(int percent, string message)
        {
           
        }
    }
}
