namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IProgressReporter
    {
        void Report(int percent, string message);
    }

}
