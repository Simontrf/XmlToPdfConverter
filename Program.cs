using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string config = "Release";
            string solutionDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            if (!Directory.Exists(Path.Combine(solutionDir, "XML2PDF_x64.S003")))
                solutionDir = Directory.GetParent(solutionDir).FullName;

            string outputDir = Path.Combine(solutionDir, "Package_64bits");
            string cliExe = Path.Combine(solutionDir, "XML2PDF_x64.S003", "XmlToPdfConverter.CLI", "bin", config, "XmlToPdfConverter.CLI.exe");
            string guiExe = Path.Combine(solutionDir, "XML2PDF_x64.S003", "XmlToPdfConverter.GUI", "bin", config, "XmlToPdfConverter.GUI.exe");
            string coreDll = Path.Combine(solutionDir, "XML2PDF_x64.S003", "XmlToPdfConverter.Core", "bin", config, "XmlToPdfConverter.Core.dll");
            string chromeSrc = Path.Combine(solutionDir, "XML2PDF_x64.S003", "XmlToPdfConverter.Core", "chrome");

            // Vérifie que les fichiers sources existent
            if (!File.Exists(cliExe)) throw new FileNotFoundException("Fichier CLI introuvable : " + cliExe);
            if (!File.Exists(guiExe)) throw new FileNotFoundException("Fichier GUI introuvable : " + guiExe);
            if (!File.Exists(coreDll)) throw new FileNotFoundException("Fichier Core introuvable : " + coreDll);

            // Crée le dossier Package
            if (Directory.Exists(outputDir)) Directory.Delete(outputDir, true);
            Directory.CreateDirectory(outputDir);

            // Copie des fichiers principaux
            File.Copy(cliExe, Path.Combine(outputDir, "XmlToPdfConverter.CLI.exe"), true);
            File.Copy(guiExe, Path.Combine(outputDir, "XmlToPdfConverter.GUI.exe"), true);
            File.Copy(coreDll, Path.Combine(outputDir, "XmlToPdfConverter.Core.dll"), true);

            // Copie des DLL supplémentaires (depuis CLI)
            foreach (var dll in Directory.GetFiles(Path.GetDirectoryName(cliExe), "*.dll"))
            {
                string dest = Path.Combine(outputDir, Path.GetFileName(dll));
                if (!File.Exists(dest)) File.Copy(dll, dest);
            }

            // Copie du dossier Chrome
            if (Directory.Exists(chromeSrc))
            {
                string chromeDst = Path.Combine(outputDir, "chrome");
                CopyDirectory(chromeSrc, chromeDst);
            }

            Console.WriteLine("✅ Packaging réussi ! Dossier créé : " + outputDir);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Erreur pendant le packaging : " + ex.Message);
        }
    }

    static void CopyDirectory(string sourceDir, string targetDir)
    {
        foreach (string dir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(dir.Replace(sourceDir, targetDir));

        foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            File.Copy(file, file.Replace(sourceDir, targetDir), true);
    }
}