using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlToPdfConverter.Core.Configuration;
using XmlToPdfConverter.Core.Interfaces;
using XmlToPdfConverter.Core.Services;


namespace XmlToPdfConverter.GUI
{
    public partial class MainForm : Form
    {
        
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
        private Label _statusLabel;
        private GuiProgressReporter _progressReporter;
        private DateTime _conversionStartTime;

        public MainForm()
        {
            InitializeComponent();

            _appConfig = new AppConfiguration();

            var logger = new GuiLogger(rtbLog);
            _resourceManager = new ResourceManager(logger);
            _conversionService = new ChromeConversionService(logger, _appConfig, _resourceManager);

            CheckDependencies();
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

            Label lblStatus = new Label
            {
                Text = "Prêt à convertir",
                Location = new Point(140, yPos - 5),
                Size = new Size(470, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.DarkBlue,
                AutoEllipsis = true
            };
            mainPanel.Controls.Add(lblStatus);

            _statusLabel = lblStatus;

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

                    if (string.IsNullOrEmpty(txtOutputDir.Text))
                    {
                        txtOutputDir.Text = Path.GetDirectoryName(ofd.FileName);
                        //outputDirectoryPath = Path.GetDirectoryName(ofd.FileName);
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

                    if (string.IsNullOrEmpty(txtOutputDir.Text))
                    {
                        txtOutputDir.Text = Path.GetDirectoryName(ofd.FileName);
                        //outputDirectoryPath = Path.GetDirectoryName(ofd.FileName);
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
                    txtOutputDir.Text = sfd.FileName;
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

            // Désactiver le bouton et préparer l'interface
            btnConvert.Enabled = false;
            btnConvert.Text = "Conversion en cours...";
            btnConvert.BackColor = Color.Orange;

            // Créer le système de progression adaptatif
            _progressReporter = new GuiProgressReporter(progressBar, _statusLabel,
                new GuiLogger(rtbLog), txtXmlFile.Text, txtXslFile.Text);

            // Afficher l'estimation de durée
            var estimatedDuration = _progressReporter.GetEstimatedDuration();
            LogMessage($"🔄 Début de conversion (durée estimée: {FormatDuration(estimatedDuration)})");

            // Démarrer en mode marquee pour l'initialisation
            _progressReporter.SetMarqueeMode(true);
            _conversionStartTime = DateTime.Now;

            try
            {
                // Créer le progress reporter pour le service
                var progress = new Progress<ConversionProgress>(p =>
                {
                    var elapsed = DateTime.Now - _conversionStartTime;
                    int simplePercent = CalculateSimpleProgress(p.Percentage, elapsed, estimatedDuration);

                    _progressReporter.Report(simplePercent, p.CurrentStep);
                });

                // Ajouter un feedback de démarrage
                _progressReporter.Report(0, "Initialisation de la conversion...");

                var conversionResult = await _conversionService.ConvertAsync(
                    txtXmlFile.Text,
                    txtXslFile.Text,
                    txtOutputDir.Text,
                    progress
                );

                if (conversionResult.Success)
                {
                    _progressReporter.Complete();
                    LogMessage($"✅ Conversion réussie en {FormatDuration(conversionResult.Duration)}!");

                    if (chkOpenResult.Checked)
                    {
                        try
                        {
                            // ✅ Attendre que le fichier soit complètement écrit
                            await Task.Delay(1000);

                            if (File.Exists(conversionResult.OutputPath))
                            {
                                var startInfo = new ProcessStartInfo
                                {
                                    FileName = conversionResult.OutputPath,
                                    UseShellExecute = true,
                                    Verb = "open"
                                };
                                Process.Start(startInfo);
                                LogMessage($"📄 Ouverture du PDF...");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMessage($"⚠ Impossible d'ouvrir le PDF: {ex.Message}");
                            LogMessage($"💡 Ouvrez manuellement: {conversionResult.OutputPath}");
                        }
                    }

                    MessageBox.Show($"Conversion réussie!\nTaille: {conversionResult.FileSizeBytes:N0} octets\nDurée: {FormatDuration(conversionResult.Duration)}",
                                   "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _progressReporter.Reset();
                    LogMessage($"❌ Conversion échouée: {conversionResult.ErrorMessage}");
                    MessageBox.Show($"Conversion échouée: {conversionResult.ErrorMessage}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (TimeoutException)
            {
                _progressReporter?.Reset();
                LogMessage("⏰ Timeout: La conversion a pris trop de temps");
                MessageBox.Show("La conversion a pris trop de temps et a été annulée.\nVérifiez la taille et la complexité de vos fichiers.",
                               "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _progressReporter?.Reset();
                LogMessage($"💥 Erreur critique: {ex.Message}");
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restaurer l'interface
                btnConvert.Enabled = true;
                btnConvert.Text = "Convertir en PDF";
                btnConvert.BackColor = Color.FromArgb(0, 120, 215);

                // Nettoyer le progress reporter
                _progressReporter = null;
            }
        }

        // Méthode pour calculer le pourcentage adapté basé sur l'estimation
        private int CalculateSimpleProgress(int reportedPercent, TimeSpan elapsed, TimeSpan estimated)
        {
            // Progression temporelle simple
            int timeBasedPercent = 0;
            if (estimated.TotalSeconds > 0)
            {
                timeBasedPercent = (int)((elapsed.TotalSeconds / estimated.TotalSeconds) * 90); // Max 90%
                timeBasedPercent = Math.Min(90, timeBasedPercent);
            }

            // ✅ LOGIQUE SIMPLE :
            if (reportedPercent == 0) // Chrome pas encore démarré ou en préparation
            {
                // Progression basée uniquement sur le temps (0-10% les 3 premières secondes)
                return Math.Min(10, (int)(elapsed.TotalSeconds * 3));
            }
            else if (reportedPercent < 75) // Chrome en cours d'exécution
            {
                // Progression mixte : 50% temps + 50% rapport Chrome
                return Math.Max(reportedPercent, (timeBasedPercent + reportedPercent) / 2);
            }
            else // Chrome terminé, attente PDF
            {
                // Utiliser directement le rapport de Chrome (75-100%)
                return Math.Max(75, reportedPercent);
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