using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlToPdfConverter.Core.Configuration;
using XmlToPdfConverter.Core.Engine;


namespace XmlToPdfConverter.GUI
{
    public partial class MainForm : Form
    {

        private string xmlFilePath = "";
        private string xslFilePath = "";
        private string outputDirectoryPath = "";
        private string chromePath = "";
        private string basePath = "";
        private string chromeProfile = ""; // Persistant
        private readonly Process chromeProcess; // Processus Chrome persistant
        private readonly ChromePathResolver _chromeResolver;

        // Contrôles UI
        private TextBox txtXmlFile;
        private TextBox txtXslFile;
        private TextBox txtOutputDir;
        private CheckBox chkOpenResult;
        private Button btnConvert;
        private Button btnBrowseXml;
        private Button btnBrowseXsl;
        private Button btnBrowseOutput;
        private ProgressBar progressBar;
        private RichTextBox rtbLog;

        public MainForm()
        {
            InitializeComponent();
            _chromeResolver = new ChromePathResolver(new GuiLogger(rtbLog));

            InitializePaths();
            CheckDependencies();
            InitializeChromeProfile();
        }

        private void InitializePaths()
        {
            basePath = Application.StartupPath;

            try
            {
                chromePath = _chromeResolver.GetChromeExecutablePath();
                LogMessage($"✓ Chrome trouvé via resolver: {chromePath}");
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Erreur résolution Chrome: {ex.Message}");
                chromePath = null;
            }

            // Profil temporaire (comme CLI)
            chromeProfile = Path.Combine(Path.GetTempPath(), "chrome-profile-gui-" + Guid.NewGuid());
        }

        private void InitializeChromeProfile()
        {
            try
            {
                if (!Directory.Exists(chromeProfile))
                {
                    Directory.CreateDirectory(chromeProfile);
                    LogMessage("✓ Profil Chrome persistant créé");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"⚠ Erreur création profil: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configuration de la fenêtre principale
            this.Text = "Convertisseur XML vers PDF (Chrome) - Optimisé";
            this.Size = new Size(720, 780);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 830);
            this.Font = new Font("Segoe UI", 9F);

            // Panel principal
            Panel mainPanel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            this.Controls.Add(mainPanel);

            int yPos = 10;

            // Titre
            Label titleLabel = new Label
            {
                Text = "Convertisseur XML vers PDF (Chrome) - Version Optimisée",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(10, yPos),
                Size = new Size(600, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            mainPanel.Controls.Add(titleLabel);
            yPos += 40;

            // Description
            Label descLabel = new Label
            {
                Text = "Conversion rapide XML + XSL vers PDF avec optimisations Chrome",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(10, yPos),
                Size = new Size(600, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            mainPanel.Controls.Add(descLabel);
            yPos += 35;

            // Section fichiers d'entrée
            Label sectionLabel = new Label
            {
                Text = "Fichiers d'entrée:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, yPos),
                Size = new Size(200, 25)
            };
            mainPanel.Controls.Add(sectionLabel);
            yPos += 30;

            // Fichier XML
            Label xmlLabel = new Label
            {
                Text = "Fichier XML :",
                Location = new Point(10, yPos),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainPanel.Controls.Add(xmlLabel);

            txtXmlFile = new TextBox
            {
                Location = new Point(120, yPos),
                Width = 430
            };
            mainPanel.Controls.Add(txtXmlFile);

            btnBrowseXml = new Button
            {
                Text = "Parcourir",
                Location = new Point(560, yPos),
                Size = new Size(150, 25)
            };
            btnBrowseXml.Click += BtnBrowseXml_Click;
            mainPanel.Controls.Add(btnBrowseXml);
            yPos += 35;

            // Fichier XSL
            Label xslLabel = new Label
            {
                Text = "Fichier XSL :",
                Location = new Point(10, yPos),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainPanel.Controls.Add(xslLabel);

            txtXslFile = new TextBox
            {
                Location = new Point(120, yPos),
                Width = 430
            };
            mainPanel.Controls.Add(txtXslFile);

            btnBrowseXsl = new Button
            {
                Text = "Parcourir",
                Location = new Point(560, yPos),
                Size = new Size(150, 25)
            };
            btnBrowseXsl.Click += BtnBrowseXsl_Click;
            mainPanel.Controls.Add(btnBrowseXsl);
            yPos += 35;


            // Dossier de sortie
            Label outputLabel = new Label
            {
                Text = "Dossier de sortie :",
                Location = new Point(10, yPos),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            mainPanel.Controls.Add(outputLabel);

            txtOutputDir = new TextBox
            {
                Location = new Point(120, yPos),
                Width = 430
            };
            mainPanel.Controls.Add(txtOutputDir);

            btnBrowseOutput = new Button
            {
                Text = "Sauvegarder sous",
                Location = new Point(560, yPos),
                Size = new Size(150, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnBrowseOutput.Click += BtnBrowseOutput_Click;
            mainPanel.Controls.Add(btnBrowseOutput);
            yPos += 45;

            // Section options
            Label optionsLabel = new Label
            {
                Text = "Options d'optimisation :",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, yPos),
                Size = new Size(200, 25)
            };
            mainPanel.Controls.Add(optionsLabel);
            yPos += 30;

            // Options de conversion
            chkOpenResult = new CheckBox
            {
                Text = "Ouvrir le PDF après conversion",
                Location = new Point(10, yPos),
                Size = new Size(300, 25),
                Checked = true
            };
            mainPanel.Controls.Add(chkOpenResult);
            yPos += 30;

            // Boutons de conversion
            btnConvert = new Button
            {
                Text = "Convertir en PDF",
                Location = new Point(10, yPos),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnConvert.Click += BtnConvert_Click;
            mainPanel.Controls.Add(btnConvert);

            // Barre de progression
            progressBar = new ProgressBar
            {
                Location = new Point(10, yPos),
                Width = 600,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 0
            };
            mainPanel.Controls.Add(progressBar);
            yPos += 40;

            // Zone de log
            Label logLabel = new Label
            {
                Text = "Journa :",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, yPos),
                Size = new Size(200, 25)
            };
            mainPanel.Controls.Add(logLabel);
            yPos += 25;

            rtbLog = new RichTextBox
            {
                Location = new Point(10, yPos),
                Size = new Size(675, 400),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Consolas", 9F),
                ReadOnly = true,
                BackColor = Color.White
            };
            mainPanel.Controls.Add(rtbLog);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Nettoyer le processus Chrome s'il existe
            CleanupChromeProcess();
            CleanupChromeProfile();
            base.OnFormClosing(e);
        }

        private void CleanupChromeProcess()
        {
            try
            {
                if (chromeProcess != null && !chromeProcess.HasExited)
                {
                    chromeProcess.Kill();
                    chromeProcess.Dispose();
                    LogMessage("✓ Processus Chrome nettoyé");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"⚠ Erreur nettoyage Chrome: {ex.Message}");
            }
        }

        private void CleanupChromeProfile()
        {
            try
            {
                if (Directory.Exists(chromeProfile) && chromeProfile.Contains("chrome-profile-gui-"))
                {
                    Directory.Delete(chromeProfile, true);
                    LogMessage("✓ Profil Chrome temporaire supprimé");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"⚠ Erreur suppression profil: {ex.Message}");
            }
        }
        private void BtnBrowseXml_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Sélectionner le fichier XML";
                ofd.Filter = "Fichiers XML (*.xml)|*.xml|Tous les fichiers (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtXmlFile.Text = ofd.FileName;
                    xmlFilePath = ofd.FileName;

                    if (string.IsNullOrEmpty(txtOutputDir.Text))
                    {
                        txtOutputDir.Text = Path.GetDirectoryName(ofd.FileName);
                        outputDirectoryPath = Path.GetDirectoryName(ofd.FileName);
                    }
                }
            }
        }

        private void BtnBrowseXsl_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Sélectionner le fichier XSL";
                ofd.Filter = "Fichiers XSL (*.xsl)|*.xsl|Tous les fichiers (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtXslFile.Text = ofd.FileName;
                    xslFilePath = ofd.FileName;

                    if (string.IsNullOrEmpty(txtOutputDir.Text))
                    {
                        txtOutputDir.Text = Path.GetDirectoryName(ofd.FileName);
                        outputDirectoryPath = Path.GetDirectoryName(ofd.FileName);
                    }
                }
            }
        }

        private void BtnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Enregistrer le PDF sous";
                sfd.Filter = "Fichiers PDF (*.pdf)|*.pdf|Tous les fichiers (*.*)|*.*";
                sfd.DefaultExt = "pdf";

                // Nom suggéré basé sur le XML sélectionné
                if (!string.IsNullOrEmpty(txtXmlFile.Text))
                {
                    string baseName = Path.GetFileNameWithoutExtension(txtXmlFile.Text);
                    sfd.FileName = baseName + ".pdf";
                }

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputDir.Text = sfd.FileName; // Maintenant c'est le chemin complet
                    outputDirectoryPath = sfd.FileName;
                }
            }
        }

        private void LogMessage(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action<string>(LogMessage), message);
                return;
            }

            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            rtbLog.AppendText($"[{timestamp}] {message}\n");
            rtbLog.ScrollToCaret();
            Application.DoEvents();
        }

        private void CheckDependencies()
        {
            LogMessage("Vérification de Chrome via ChromePathResolver...");

            if (_chromeResolver.IsChromeAvailable())
            {
                LogMessage($"✓ Chrome disponible: {chromePath}");
            }
            else
            {
                LogMessage("✗ Chrome non disponible");
                LogMessage("→ Vérifiez l'installation de Chrome portable");
                chromePath = null;
            }
        }

        private bool ValidateInputs()
        {
            // Vérifier si le fichier XML est spécifié
            if (string.IsNullOrEmpty(txtXmlFile.Text))
            {
                MessageBox.Show("Veuillez sélectionner un fichier XML", "Attention",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Vérifier si le fichier XML existe
            if (!File.Exists(txtXmlFile.Text))
            {
                MessageBox.Show("Le fichier XML spécifié n'existe pas", "Erreur",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Vérifier si le fichier XSL est spécifié
            if (string.IsNullOrEmpty(txtXslFile.Text))
            {
                MessageBox.Show("Veuillez sélectionner un fichier XSL", "Attention",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Vérifier si le fichier XSL existe
            if (!File.Exists(txtXslFile.Text))
            {
                MessageBox.Show("Le fichier XSL spécifié n'existe pas", "Erreur",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Vérifier si le dossier de sortie est spécifié
            if (string.IsNullOrEmpty(txtOutputDir.Text))
            {
                MessageBox.Show("Veuillez sélectionner un dossier de sortie", "Attention",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Vérifier si le dossier de sortie existe, sinon essayer de le créer
            string outputDir = Path.GetDirectoryName(txtOutputDir.Text);
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                    LogMessage($"✓ Dossier de sortie créé: {outputDir}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Impossible de créer le dossier de sortie:\n{ex.Message}",
                                   "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Vérifier la cohérence des chemins XML/XSL
            try
            {
                string xmlDir = Path.GetDirectoryName(Path.GetFullPath(txtXmlFile.Text));
                string xslPath = Path.GetFullPath(txtXslFile.Text);

                // Tester si le XSL est accessible depuis le répertoire XML
                if (!File.Exists(xslPath))
                {
                    MessageBox.Show($"Le fichier XSL n'est pas accessible : {xslPath}",
                                   "Erreur de chemin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de validation des chemins : {ex.Message}",
                               "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!_chromeResolver.IsChromeAvailable())
            {
                MessageBox.Show("Chrome n'est pas disponible.\n" +
                               "Vérifiez l'installation de Chrome portable.",
                               "Chrome manquant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Mettre à jour les variables de chemin
            xmlFilePath = txtXmlFile.Text;
            outputDirectoryPath = txtOutputDir.Text;

            return true;
        }

        private void BtnCheckXml_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtXmlFile.Text))
            {
                MessageBox.Show("Veuillez d'abord sélectionner un fichier XML", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string xmlPath = txtXmlFile.Text;
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("Le fichier XML n'existe pas", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string content = File.ReadAllText(xmlPath, Encoding.UTF8);

                string xslPattern = @"<\?xml-stylesheet[^>]*href=['""]([^'""]*)['""][^>]*\?>";
                MatchCollection matches = Regex.Matches(content, xslPattern, RegexOptions.IgnoreCase);

                if (matches.Count > 0)
                {
                    string xslRef = matches[0].Groups[1].Value;
                    LogMessage($"✓ Référence XSL trouvée: {xslRef}");

                    string xmlDir = Path.GetDirectoryName(xmlPath);
                    string xslPath = Path.Combine(xmlDir, xslRef);

                    if (File.Exists(xslPath))
                    {
                        LogMessage($"✓ Fichier XSL trouvé: {xslPath}");
                        MessageBox.Show($"XML valide avec référence XSL:\n{xslRef}\n\nFichier XSL trouvé.",
                                      "Vérification OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        LogMessage($"✗ Fichier XSL manquant: {xslPath}");
                        MessageBox.Show($"Le XML référence le fichier XSL:\n{xslRef}\n\nMais ce fichier n'existe pas à:\n{xslPath}",
                                      "Fichier XSL manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    LogMessage("✗ Aucune référence XSL trouvée dans le XML");
                    MessageBox.Show("Le fichier XML ne contient pas de référence à une feuille de style XSL.\n\n" +
                                  "Ajoutez une ligne comme:\n" +
                                  "<?xml-stylesheet type=\"text/xsl\" href=\"votre-fichier.xsl\"?>",
                                  "Pas de XSL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Erreur lors de la vérification: {ex.Message}");
                MessageBox.Show($"Impossible de lire le fichier XML:\n{ex.Message}",
                              "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool XmlToPdfChromeOptimized(string xmlPath, string pdfPath)
        {
            var stopwatch = Stopwatch.StartNew();
            LogMessage("🚀 Conversion XML vers PDF optimisée...");

            try
            {
                string xmlUrl = new Uri(Path.GetFullPath(xmlPath)).AbsoluteUri;

                string outputDir = Path.GetDirectoryName(pdfPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Arguments Chrome optimisés SANS console
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = chromePath,
                    Arguments = string.Join(" ", ChromeArguments.GetChromeArguments(
                        pdfPath,
                        xmlUrl,
                        chromeProfile
                    )),

                    UseShellExecute = false,
                    CreateNoWindow = true,           // CRITIQUE: Empêche la création de fenêtre
                    WindowStyle = ProcessWindowStyle.Hidden, // AJOUT: Force la fenêtre à être cachée
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                LogMessage($"URL XML: {xmlUrl}");
                LogMessage($"PDF de sortie: {pdfPath}");

                using (Process process = Process.Start(startInfo))
                {
                    // Lecture asynchrone pour éviter les blocages
                    Task<string> errorTask = process.StandardError.ReadToEndAsync();
                    Task<string> outputTask = process.StandardOutput.ReadToEndAsync();

                    bool finished = process.WaitForExit(600000);

                    if (!finished)
                    {
                        process.Kill();
                        LogMessage("✗ Timeout lors de la conversion Chrome (60s)");
                        return false;
                    }

                    LogMessage($"Code de retour Chrome: {process.ExitCode}");

                    // Lecture des erreurs uniquement si nécessaire
                    try
                    {
                        string stderr = errorTask.Result;
                        if (!string.IsNullOrWhiteSpace(stderr))
                        {
                            var importantErrors = stderr.Split('\n')
                                .Where(line => !string.IsNullOrEmpty(line) &&
                                             !line.ToLower().Contains("devtools") &&
                                             !line.ToLower().Contains("extensions") &&
                                             !line.ToLower().Contains("renderer") &&
                                             !line.ToLower().Contains("google is not defined") &&
                                             !line.ToLower().Contains("uncaught referenceerror: google") &&
                                             !line.ToLower().Contains("console") &&
                                             !line.ToLower().Contains("warning"))
                                .Take(2);

                            if (importantErrors.Any())
                            {
                                LogMessage($"⚠ Erreurs: {string.Join(" | ", importantErrors)}");
                            }
                        }
                    }
                    catch
                    {
                        // Ignorer les erreurs de lecture des streams
                    }
                }

                // Vérification optimisée de la création du PDF
                int maxWait = 15;
                for (int i = 0; i < maxWait; i++)
                {
                    if (File.Exists(pdfPath))
                    {
                        FileInfo fileInfo = new FileInfo(pdfPath);
                        if (fileInfo.Length > 0)
                        {
                            stopwatch.Stop();
                            LogMessage($"✓ PDF créé en {stopwatch.ElapsedMilliseconds}ms ({fileInfo.Length} octets)");
                            return true;
                        }
                    }
                    Thread.Sleep(500); // Vérification toutes les 500ms
                }

                LogMessage("✗ PDF non créé dans les temps");
                return false;
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Erreur conversion: {ex.Message}");
                return false;
            }
        }
        private async void BtnConvert_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            btnConvert.Enabled = false;
            progressBar.MarqueeAnimationSpeed = 30;

            try
            {
                // Exécuter la conversion dans un thread en arrière-plan
                bool success = await Task.Run(() => ConvertToPdfBackground());

                // Ces lignes s'exécutent dans le thread UI principal
                if (success)
                {
                    LogMessage("✅ Conversion terminée avec succès!");
                }
                else
                {
                    LogMessage("❌ Échec de la conversion");
                    MessageBox.Show("Conversion échouée", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"💥 Erreur inattendue: {ex.Message}");
                MessageBox.Show($"Erreur inattendue: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Réactiver les contrôles
                btnConvert.Enabled = true;
                progressBar.MarqueeAnimationSpeed = 0;
            }
        }

        // Nouvelle méthode qui ne touche PAS à l'UI
        private bool ConvertToPdfBackground()
        {

            var totalStopwatch = Stopwatch.StartNew();

            try
            {
                string xmlPath = txtXmlFile.Text;
                string baseName = Path.GetFileNameWithoutExtension(xmlPath);
                string pdfPath = txtOutputDir.Text;

                // Prétraitement XML
                var preprocessor = new XmlPreprocessor();
                string xslFile = txtXslFile.Text; // Utiliser le chemin XSL sélectionné
                string preprocessedXml = preprocessor.Preprocess(xmlPath, xslFile, new GuiLogger(rtbLog));

                // Utiliser Invoke pour les messages de log
                this.Invoke(new Action(() => LogMessage($"🎯 Début de la conversion optimisée de {baseName}")));

                bool success = XmlToPdfChromeOptimized(preprocessedXml, pdfPath);

                if (!success)
                {
                    this.Invoke(new Action(() => LogMessage("✗ Échec de la conversion PDF")));
                    return false;
                }

                if (!File.Exists(pdfPath) || new FileInfo(pdfPath).Length == 0)
                {
                    this.Invoke(new Action(() => LogMessage("✗ PDF manquant ou vide")));
                    return false;
                }

                totalStopwatch.Stop();
                var fileSize = new FileInfo(pdfPath).Length;

                if (File.Exists(preprocessedXml))
                    File.Delete(preprocessedXml);

                this.Invoke(new Action(() =>
                {
                    LogMessage($"🏆 PDF généré avec succès en {totalStopwatch.ElapsedMilliseconds}ms ({fileSize} octets)");

                    // Ouvrir le PDF si demandé
                    if (chkOpenResult.Checked)
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                            LogMessage("✓ PDF ouvert");
                        }
                        catch (Exception)
                        {
                            LogMessage("⚠ Impossible d'ouvrir le PDF automatiquement");
                        }
                    }

                    LogMessage($"✅ Conversion terminée: {pdfPath}");
                    MessageBox.Show($"Conversion réussie en {totalStopwatch.ElapsedMilliseconds}ms!\nFichier: {pdfPath}",
                                   "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));

                return true;
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => LogMessage($"💥 Erreur inattendue: {ex.Message}")));
                return false;
            }
        }

        // Supprimez complètement votre ancienne méthode ConvertToPdf()
        // et remplacez SetProgressBarActive par celle-ci :
        private void SetProgressBarActive(bool active)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<bool>(SetProgressBarActive), active);
                return;
            }
            progressBar.MarqueeAnimationSpeed = active ? 30 : 0;
        }
    }
}