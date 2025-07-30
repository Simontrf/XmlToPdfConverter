using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Engine
{
    public class XmlPreprocessor : IXmlPreprocessor
    {
        public string Preprocess(string xmlInputPath, string xslInputPath, ILogger logger)
        {
            string xmlContent = File.ReadAllText(xmlInputPath, Encoding.UTF8);

            bool hasXmlDeclaration = xmlContent.Contains("<?xml version");

            // Calculer le chemin relatif du XSL par rapport au XML
            string xmlDir = Path.GetDirectoryName(Path.GetFullPath(xmlInputPath));
            string xslDir = Path.GetDirectoryName(Path.GetFullPath(xslInputPath));
            string xslFileName = Path.GetFileName(xslInputPath);

            string relativePath;
            if (xmlDir.Equals(xslDir, StringComparison.OrdinalIgnoreCase))
            {
                // Même dossier
                relativePath = xslFileName;
            }
            else
            {
                // Calculer le chemin relatif ou utiliser le chemin absolu en URI
                relativePath = new Uri(Path.GetFullPath(xslInputPath)).AbsoluteUri;
            }

            // SUPPRIMER toutes les références XSL existantes
            string pattern = @"<\?xml-stylesheet[^>]*\?>\s*";
            xmlContent = Regex.Replace(xmlContent, pattern, "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            logger?.Log("Suppression de toutes les références XSL existantes", LogLevel.Debug);

            // Créer le nouveau contenu XML
            StringBuilder newContent = new StringBuilder();

            // Ajouter la déclaration XML si manquante
            if (!hasXmlDeclaration)
            {
                newContent.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                logger?.Log("Ajout de l'en-tête XML", LogLevel.Debug);
            }

            // TOUJOURS ajouter la nouvelle référence XSL
            newContent.AppendLine($"<?xml-stylesheet type=\"text/xsl\" href=\"{relativePath}\"?>");
            logger?.Log($"Ajout de la nouvelle référence XSL : {relativePath}", LogLevel.Debug);

            // Ajouter le contenu XML (en supprimant les lignes vides au début si présentes)
            string cleanedXmlContent = xmlContent.TrimStart('\r', '\n', ' ', '\t');
            newContent.Append(cleanedXmlContent);

            // Créer le fichier temporaire
            string xslParentFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(xslInputPath)));
            string tempFile = Path.Combine(xslParentFolder, "preprocessed_" + Guid.NewGuid() + ".xml");

            File.WriteAllText(tempFile, newContent.ToString(), Encoding.UTF8);

            logger?.Log("Fichier XML prétraité avec XSL forcé : " + tempFile, LogLevel.Info);

            return tempFile;
        }
    }
}