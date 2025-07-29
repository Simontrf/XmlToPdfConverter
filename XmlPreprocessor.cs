using System;
using System.IO;
using System.Text;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Engine
{
    public class XmlPreprocessor : IXmlPreprocessor
    {
        public string Preprocess(string xmlInputPath, string xslInputPath, ILogger logger)
        {
            string[] lines = File.ReadAllLines(xmlInputPath);
            bool hasXmlDeclaration = false;
            bool hasXslReference = false;

            foreach (var line in lines)
            {
                if (line.Contains("<?xml version"))
                    hasXmlDeclaration = true;
                if (line.Contains("<?xml-stylesheet"))
                    hasXslReference = true;
            }

            string xslParentFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(xslInputPath)));
            string tempFile = Path.Combine(xslParentFolder, "preprocessed_" + Guid.NewGuid() + ".xml");
            using (StreamWriter writer = new StreamWriter(tempFile, false, Encoding.UTF8))
            {
                if (!hasXmlDeclaration)
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    logger?.Log("Ajout de l'en-tête XML", LogLevel.Debug);
                }

                if (!hasXslReference)
                {
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
                        // Calculer le chemin relatif
                        Uri xmlUri = new Uri(xmlDir + Path.DirectorySeparatorChar);
                        relativePath = new Uri(Path.GetFullPath(xslInputPath)).AbsoluteUri;
                    }

                    writer.WriteLine($"<?xml-stylesheet type=\"text/xsl\" href=\"{relativePath}\"?>");
                    logger?.Log($"Référence XSL relative : {relativePath}", LogLevel.Debug);
                }

                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }

            logger?.Log("Fichier XML prétraité : " + tempFile, LogLevel.Info);

            return tempFile;
        }
    }
}
