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

            // Calculer le chemin relatif du XSL
            string xmlDir = Path.GetDirectoryName(Path.GetFullPath(xmlInputPath));
            string xslDir = Path.GetDirectoryName(Path.GetFullPath(xslInputPath));
            string xslFileName = Path.GetFileName(xslInputPath);

            string relativePath;
            if (xmlDir.Equals(xslDir, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = xslFileName;
            }
            else
            {
                relativePath = new Uri(Path.GetFullPath(xslInputPath)).AbsoluteUri;
            }

            // SUPPRIMER toutes les références XSL existantes
            string pattern = @"<\?xml-stylesheet[^>]*\?>\s*";
            xmlContent = Regex.Replace(xmlContent, pattern, "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            logger?.Log("Suppression de toutes les références XSL existantes", LogLevel.Debug);

            // Nouvelle référence XSL à insérer
            string newXslReference = $"<?xml-stylesheet type=\"text/xsl\" href=\"{relativePath}\"?>";

            StringBuilder newContent = new StringBuilder();

            // Détecter et préserver la déclaration XML existante
            Match xmlDeclarationMatch = Regex.Match(xmlContent, @"<\?xml[^>]*\?>", RegexOptions.IgnoreCase);

            if (xmlDeclarationMatch.Success)
            {
                // Garder la déclaration existante
                string xmlDeclaration = xmlDeclarationMatch.Value.Trim().Replace("\\\"", "\"");
                newContent.AppendLine(xmlDeclaration);
                logger?.Log("Déclaration XML existante préservée", LogLevel.Debug);

                // Ajouter la référence XSL après
                newContent.AppendLine(newXslReference);

                // Ajouter le reste du contenu (sans la déclaration XML)
                string remainingContent = xmlContent.Substring(xmlDeclarationMatch.Index + xmlDeclarationMatch.Length).TrimStart();
                newContent.Append(remainingContent);
            }
            else
            {
                // Ajouter une déclaration XML par défaut
                newContent.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                logger?.Log("Ajout de l'en-tête XML par défaut", LogLevel.Debug);

                // Ajouter la référence XSL
                newContent.AppendLine(newXslReference);

                // Ajouter le contenu XML nettoyé
                string cleanedXmlContent = xmlContent.TrimStart('\r', '\n', ' ', '\t');
                newContent.Append(cleanedXmlContent);
            }

            logger?.Log($"Nouvelle référence XSL ajoutée : {relativePath}", LogLevel.Debug);

            // Créer le fichier temporaire
            string xslParentFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(xslInputPath)));
            string tempFile = Path.Combine(xslParentFolder, "preprocessed_" + Guid.NewGuid() + ".xml");

            File.WriteAllText(tempFile, newContent.ToString(), Encoding.UTF8);

            logger?.Log("Fichier XML prétraité avec XSL forcé : " + tempFile, LogLevel.Info);

            return tempFile;
        }
    }
}