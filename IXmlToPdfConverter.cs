namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IXmlToPdfConverter
    {
        bool Convert(string xmlPath, string originalXmlPath, IProgressReporter progress, ILogger logger);
    }

}
