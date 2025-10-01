namespace XmlToPdfConverter.Core.Interfaces
{
    //Instance de prétraitement de fichier XML (injection référence XSl en en-tête XML)
    public interface IXmlPreprocessor
    {
        string Preprocess(string xmlInputPath, string xslInputPath, ILogger logger); //xmlInputPath = fichier XML source, xslInputPath = fichier XSL à référencier
    }
}

