namespace XmlToPdfConverter.Core.Configuration
{
    public interface IChromePathResolver
    {
        string GetChromeExecutablePath();
        bool IsChromeAvailable();
    }
}