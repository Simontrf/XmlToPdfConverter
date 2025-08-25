using XmlToPdfConverter.Core.Configuration;

namespace XmlToPdfConverter.Core.Engine
{
    public static class ChromeArguments
    {
        [System.Obsolete("Utilisez AppConfiguration.ChromeSettings.GetArguments() à la place")]
        public static string[] GetChromeArguments(string pdfPath, string xmlUrl, string profilePath)
        {
            // Déléguer à la nouvelle configuration pour la compatibilité
            var settings = new ChromeSettings();
            return settings.GetArguments(pdfPath, xmlUrl, profilePath);
        }
    }
}