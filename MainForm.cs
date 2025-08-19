using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        private CancellationTokenSource _conversionCancellation;

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
                // ✅ NETTOYER l'ancien profil s'il existe
                if (Directory.Exists(chromeProfile))
                {
                    Directory.Delete(chromeProfile, true);
                    Thread.Sleep(1000); // Attendre la suppression
                }

                // ✅ CRÉER un nouveau profil
                Directory.CreateDirectory(chromeProfile);
                LogMessage("✓ Profil Chrome créé/nettoyé");
            }
            catch (Exception ex)
            {
                LogMessage($"⚠ Erreur profil: {ex.Message}");
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
                Text = "Journal :",
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

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
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

        private async void BtnConvert_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            // Si une conversion est en cours, permettre l'annulation
            if (_conversionCancellation != null && !_conversionCancellation.Token.IsCancellationRequested)
            {
                var result = MessageBox.Show("Une conversion est en cours. Voulez-vous l'annuler ?",
                                           "Annuler la conversion",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _conversionCancellation.Cancel();
                    LogMessage("🛑 Annulation demandée par l'utilisateur");
                    return;
                }
                else
                {
                    return; // Ne pas démarrer une nouvelle conversion
                }
            }

            btnConvert.Text = "Annuler";
            btnConvert.BackColor = Color.FromArgb(220, 53, 69); // Rouge
            progressBar.MarqueeAnimationSpeed = 30;

            _conversionCancellation = new CancellationTokenSource();

            try
            {
                // Exécuter la conversion dans un thread en arrière-plan
                bool success = await Task.Run(() => ConvertToPdfBackground(_conversionCancellation.Token),
                                             _conversionCancellation.Token);

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
            catch (OperationCanceledException)
            {
                LogMessage("🛑 Conversion annulée par l'utilisateur");
                CleanupChromeProcess();
                CleanupChromeProfile();
                MessageBox.Show("Conversion annulée", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"💥 Erreur inattendue: {ex.Message}");
                MessageBox.Show($"Erreur inattendue: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Réactiver les contrôles
                btnConvert.Text = "Convertir en PDF";
                btnConvert.BackColor = Color.FromArgb(0, 120, 215); // Bleu original
                btnConvert.Enabled = true;
                progressBar.MarqueeAnimationSpeed = 0;
                _conversionCancellation = null;
            }
        }

        private bool XmlToPdfChromeOptimized(string xmlPath, string pdfPath, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            LogMessage("🚀 Conversion XML vers PDF optimisée avec possibilité d'annulation...");

            // ✅ TUER TOUS LES CHROME EXISTANTS
            try
            {
                var existingChromes = Process.GetProcessesByName("chrome");
                foreach (var proc in existingChromes)
                {
                    if (!proc.HasExited)
                    {
                        proc.Kill();
                        proc.WaitForExit(2000);
                    }
                    proc.Dispose();
                }
                LogMessage($"✓ {existingChromes.Length} processus Chrome nettoyés");
            }
            catch (Exception ex)
            {
                LogMessage($"⚠ Erreur nettoyage Chrome: {ex.Message}");
            }

            // Attendre un peu que la mémoire soit libérée
            Thread.Sleep(3000);

            try
            {
                string xmlUrl = new Uri(Path.GetFullPath(xmlPath)).AbsoluteUri;

                string outputDir = Path.GetDirectoryName(pdfPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = chromePath,
                    Arguments = string.Join(" ", ChromeArguments.GetChromeArguments(
                        pdfPath,
                        xmlUrl,
                        chromeProfile
                    )),

                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                LogMessage($"URL XML: {xmlUrl}");
                LogMessage($"PDF de sortie: {pdfPath}");
                LogMessage("⏳ Lancement de Chrome...");

                using (Process process = Process.Start(startInfo))
                {
                    Task<string> errorTask = process.StandardError.ReadToEndAsync();
                    Task<string> outputTask = process.StandardOutput.ReadToEndAsync();

                    var feedbackStopwatch = Stopwatch.StartNew();
                    int feedbackInterval = 120000; // 2 minutes

                    while (!process.HasExited)
                    {
                        // Vérifier l'annulation
                        cancellationToken.ThrowIfCancellationRequested();

                        if (process.WaitForExit(feedbackInterval))
                        {
                            break;
                        }

                        var elapsed = feedbackStopwatch.Elapsed;
                        LogMessage($"⏳ Chrome en cours... ({elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}) [Cliquez 'Annuler' pour arrêter]");
                    }

                    var elapsed1 = feedbackStopwatch.Elapsed;
                    LogMessage($"✓ Chrome terminé après {elapsed1.Hours:D2}h{elapsed1.Minutes:D2}m{elapsed1.Seconds:D2}s");
                }

                // ✅ FORCER la libération mémoire
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                LogMessage("✓ Mémoire libérée manuellement");

                // Vérification infinie avec possibilité d'annulation
                LogMessage("🔍 Vérification PDF...");
                var pdfCheckStopwatch = Stopwatch.StartNew();

                DateTime lastLogTime = DateTime.MinValue;

                while (true)
                {
                    // Vérifier l'annulation
                    cancellationToken.ThrowIfCancellationRequested();


                    if (File.Exists(pdfPath))
                    {
                        FileInfo fileInfo = new FileInfo(pdfPath);
                        if (fileInfo.Length > 0)
                        {
                            stopwatch.Stop();
                            var elapsed = pdfCheckStopwatch.Elapsed;
                            LogMessage($"✓ PDF créé en {elapsed.Minutes:D2}m{elapsed.Seconds:D2}s ({fileInfo.Length} octets)");
                            return true;
                        }
                    }

                    var now = DateTime.Now;
                    if ((now - lastLogTime).TotalMinutes >= 4)
                    {
                        lastLogTime = now;
                        var elapsed = pdfCheckStopwatch.Elapsed;
                        LogMessage($"⏳ Attente PDF... ({elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}) - [Annulable]");
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException)
            {
                LogMessage("🛑 Conversion annulée pendant l'exécution");
                CleanupChromeProcess();

                // Tuer le processus Chrome s'il existe encore
                try
                {
                    var chromeProcesses = Process.GetProcessesByName("chrome");
                    foreach (var proc in chromeProcesses)
                    {
                        if (proc.StartInfo?.Arguments?.Contains(chromeProfile) == true)
                        {
                            proc.Kill();
                            LogMessage("✓ Processus Chrome terminé");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"⚠ Erreur arrêt Chrome: {ex.Message}");
                }

                throw;
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Erreur conversion: {ex.Message}");
                return false;
            }
        }

        // Nouvelle méthode qui ne touche PAS à l'UI
        private bool ConvertToPdfBackground(CancellationToken cancellationToken = default)
        {
            var totalStopwatch = Stopwatch.StartNew();

            try
            {
                // Vérifier l'annulation avant de commencer
                cancellationToken.ThrowIfCancellationRequested();

                string xmlPath = txtXmlFile.Text;
                string baseName = Path.GetFileNameWithoutExtension(xmlPath);
                string pdfPath = txtOutputDir.Text;

                // Prétraitement XML
                this.Invoke(new Action(() => LogMessage("🔧 Prétraitement XML...")));
                var preprocessor = new XmlPreprocessor();
                string xslFile = txtXslFile.Text;
                string preprocessedXml = preprocessor.Preprocess(xmlPath, xslFile, new GuiLogger(rtbLog));

                // Vérifier l'annulation après prétraitement
                cancellationToken.ThrowIfCancellationRequested();

                this.Invoke(new Action(() => LogMessage($"🎯 Début de la conversion optimisée de {baseName}")));

                bool success = XmlToPdfChromeOptimized(preprocessedXml, pdfPath, cancellationToken);

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
            catch (OperationCanceledException)
            {
                this.Invoke(new Action(() => LogMessage("🛑 Conversion annulée")));

                // Nettoyer le fichier XML prétraité si il existe
                try
                {
                    string xmlPath = txtXmlFile.Text;
                    string preprocessedPath = Path.Combine(Path.GetTempPath(), "preprocessed_*.xml");
                    var files = Directory.GetFiles(Path.GetTempPath(), "preprocessed_*.xml");
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                    this.Invoke(new Action(() => LogMessage("✓ Fichiers temporaires nettoyés")));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => LogMessage($"⚠ Erreur nettoyage: {ex.Message}")));
                }

                throw;
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => LogMessage($"💥 Erreur inattendue: {ex.Message}")));
                return false;
            }
        }

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