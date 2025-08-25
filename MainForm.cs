using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XmlToPdfConverter.Core.Configuration;
using XmlToPdfConverter.Core.Interfaces;
using XmlToPdfConverter.Core.Services;


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
        private readonly ChromeConversionService _conversionService;
        private readonly AppConfiguration _appConfig;
        private readonly IResourceManager _resourceManager;

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

            _appConfig = new AppConfiguration();
            _resourceManager = new ResourceManager(new GuiLogger(rtbLog));

            var logger = new GuiLogger(rtbLog);
            _conversionService = new ChromeConversionService(logger, _appConfig, _resourceManager);

            CheckDependencies();
            TestNewArchitecture();
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
            _conversionService?.Dispose();
            _resourceManager?.Dispose();
            base.OnFormClosing(e);
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
            LogMessage("Vérification des dépendances...");

            if (_conversionService.IsAvailable)
            {
                LogMessage("✓ Chrome disponible via le service de conversion");
            }
            else
            {
                LogMessage("✗ Chrome non disponible");
                LogMessage("→ Vérifiez l'installation de Chrome portable");
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtXmlFile.Text))
            {
                MessageBox.Show("Veuillez sélectionner un fichier XML", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!File.Exists(txtXmlFile.Text))
            {
                MessageBox.Show("Le fichier XML spécifié n'existe pas", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtXslFile.Text))
            {
                MessageBox.Show("Veuillez sélectionner un fichier XSL", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!File.Exists(txtXslFile.Text))
            {
                MessageBox.Show("Le fichier XSL spécifié n'existe pas", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtOutputDir.Text))
            {
                MessageBox.Show("Veuillez sélectionner un dossier de sortie", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Le service se chargera de valider Chrome et créer les dossiers
            return true;
        }

        private async void BtnConvert_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                var progress = new Progress<ConversionProgress>(UpdateProgress);

                var conversionResult = await _conversionService.ConvertAsync(
                    txtXmlFile.Text,
                    txtXslFile.Text,
                    txtOutputDir.Text);

                if (conversionResult.Success)
                {
                    LogMessage($"✅ Conversion réussie en {FormatDuration(conversionResult.Duration)}!");

                    if (chkOpenResult.Checked)
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(conversionResult.OutputPath) { UseShellExecute = true });
                        }
                        catch (Exception ex)
                        {
                            LogMessage($"⚠ Impossible d'ouvrir le PDF: {ex.Message}");
                        }
                    }

                    MessageBox.Show($"Conversion réussie!\nTaille: {conversionResult.FileSizeBytes} octets\nDurée: {FormatDuration(conversionResult.Duration)}",
                                   "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    LogMessage($"❌ Conversion échouée: {conversionResult.ErrorMessage}");
                    MessageBox.Show($"Conversion échouée: {conversionResult.ErrorMessage}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                btnConvert.Text = "Convertir en PDF";
                btnConvert.BackColor = Color.FromArgb(0, 120, 215);
                progressBar.MarqueeAnimationSpeed = 0;

            }
        }

        private void UpdateProgress(ConversionProgress progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ConversionProgress>(UpdateProgress), progress);
                return;
            }

            // Optionnel : mettre à jour une barre de progression numérique
            LogMessage($"[{progress.Percentage}%] {progress.CurrentStep} (Temps écoulé: {progress.Elapsed.TotalMinutes:F1}min)");
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
        private void TestNewArchitecture()
        {
            try
            {
                LogMessage("🔧 Test de la nouvelle architecture...");
                LogMessage($"✓ Service de conversion disponible: {_conversionService.IsAvailable}");
                LogMessage($"✓ Configuration chargée: {_appConfig != null}");
                LogMessage($"✓ Resource Manager initialisé: {_resourceManager != null}");
                LogMessage("✅ Architecture validée");
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Erreur test architecture: {ex.Message}");
            }
        }
        private static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalMinutes < 1)
                return $"{duration.Seconds}s";
            else if (duration.TotalHours < 1)
                return $"{duration.Minutes}min {duration.Seconds}s";
            else
                return $"{duration.Hours}h {duration.Minutes}min {duration.Seconds}s";
        }
    }
}