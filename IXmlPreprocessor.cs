namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IXmlPreprocessor
    {
        string Preprocess(string xmlInputPath, string xslInputPath, ILogger logger);
    }
}

