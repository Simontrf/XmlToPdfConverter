using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.GUI
{
    public class GuiProgressReporter : IProgressReporter
    {
        private readonly string _xmlPath;
        private readonly string _xslPath;
        private readonly ProgressBar _progressBar;
        private readonly Label _statusLabel;
        private readonly ILogger _logger;
        private readonly long _estimatedComplexity;

        // Coefficients d'estimation basés sur la taille des fichiers
        private readonly double _baseTimeSeconds = 5.0; // Temps de base minimal
        private readonly double _xmlSizeFactor = 0.001; // 1ms par KB de XML
        private readonly double _xslComplexityFactor = 0.01; // Facteur de complexité XSL
        private DateTime _startTime;
        private DateTime _lastUpdateTime;
        private int _lastPercent = -1;
        private readonly System.Windows.Forms.Timer _smoothingTimer;
        private int _targetPercent = 0;
        private int _currentDisplayPercent = 0;

        public GuiProgressReporter(ProgressBar progressBar, Label statusLabel, ILogger logger,
                         string xmlPath, string xslPath)
        {
            _progressBar = progressBar;
            _statusLabel = statusLabel;
            _logger = logger;

            // ✅ AJOUTER: Stocker les chemins pour l'estimation
            _xmlPath = xmlPath;
            _xslPath = xslPath;

            _estimatedComplexity = CalculateComplexity(xmlPath, xslPath);
            _startTime = DateTime.Now;
            _lastUpdateTime = DateTime.Now;

            // Timer pour l'animation fluide de la barre de progression
            _smoothingTimer = new System.Windows.Forms.Timer();
            _smoothingTimer.Interval = 50; // 20 FPS
            _smoothingTimer.Tick += SmoothingTimer_Tick;
        }
        private void SmoothingTimer_Tick(object sender, EventArgs e)
        {
            if (_currentDisplayPercent < _targetPercent)
            {
                _currentDisplayPercent = Math.Min(_targetPercent, _currentDisplayPercent + 1);
                UpdateProgressBarValue(_currentDisplayPercent);

                if (_currentDisplayPercent >= _targetPercent)
                {
                    _smoothingTimer.Stop();
                }
            }
        }

        private long CalculateComplexity(string xmlPath, string xslPath)
        {
            long complexity = 0;

            try
            {
                if (File.Exists(xmlPath))
                {
                    var xmlInfo = new FileInfo(xmlPath);
                    complexity += xmlInfo.Length / 1000;

                }

                if (File.Exists(xslPath))
                {
                    var xslInfo = new FileInfo(xslPath);
                    complexity += xslInfo.Length / 100;
                }
            }
            catch (Exception ex)
            {
                _logger?.Log($"Erreur calcul complexité: {ex.Message}", LogLevel.Warning);
                complexity = 50000;
            }

            return Math.Max(complexity, 10000);
        }

        public TimeSpan GetEstimatedDuration()
        {
            try
            {
                double estimatedSeconds = 20; // Base réaliste

                if (File.Exists(_xmlPath))
                {
                    var xmlInfo = new FileInfo(_xmlPath);
                    double xmlMB = xmlInfo.Length / (1024.0 * 1024.0);

                    if (xmlMB < 1)
                    {
                        estimatedSeconds = 10 + (xmlMB * 15);
                    }
                    else if (xmlMB < 10)
                    {
                        estimatedSeconds = 15 + (xmlMB * 8);
                    }
                    else if (xmlMB < 50)
                    {
                        estimatedSeconds = 30 + (xmlMB * 3);
                    }
                    else
                    {
                        estimatedSeconds = 60 + (xmlMB * 2);
                    }

                    // ✅ SUPPRIMER: Le log debug répétitif
                    // _logger?.Log($"📊 Estimation: {xmlMB:F1}MB XML → {estimatedSeconds:F0}s", LogLevel.Debug);
                }

                return TimeSpan.FromSeconds(Math.Max(10, Math.Min(900, estimatedSeconds)));
            }
            catch (Exception ex)
            {
                _logger?.Log($"Erreur estimation: {ex.Message}", LogLevel.Warning);
                return TimeSpan.FromSeconds(60);
            }
        }

        public void Report(int percent, string message)
        {
            if (_progressBar?.InvokeRequired == true)
            {
                _progressBar.Invoke(new Action<int, string>(Report), percent, message);
                return;
            }

            try
            {
                percent = Math.Max(0, Math.Min(100, percent));

                // ✅ AJOUTER: Éviter les mises à jour trop fréquentes
                var now = DateTime.Now;
                if ((now - _lastUpdateTime).TotalMilliseconds < 200 && percent == _lastPercent)
                {
                    return; // Ignorer si même pourcentage et moins de 200ms écoulées
                }

                // Animation fluide uniquement si progression significative
                if (percent > _lastPercent + 2 || percent == 100)
                {
                    _targetPercent = percent;
                    _smoothingTimer.Start();
                    _lastPercent = percent;
                }

                // Calcul du temps restant estimé
                var elapsed = DateTime.Now - _startTime;
                string timeInfo = CalculateTimeRemaining(percent, elapsed);

                // Mise à jour du texte de statut enrichi
                if (_statusLabel != null)
                {
                    string detailedStatus = $"{message} ({percent}%) - {timeInfo}";
                    _statusLabel.Text = detailedStatus;

                    // Couleur adaptative du texte
                    if (percent < 30)
                        _statusLabel.ForeColor = Color.DarkOrange;
                    else if (percent < 70)
                        _statusLabel.ForeColor = Color.Blue;
                    else
                        _statusLabel.ForeColor = Color.DarkGreen;
                }

                // ✅ MODIFIER: Log seulement les changements significatifs
                if (percent != _lastPercent || (now - _lastUpdateTime).TotalSeconds >= 15)
                {
                    _logger?.Log($"Progression: {percent}% - {message} - {timeInfo}", LogLevel.Debug);
                }

                _lastUpdateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                _logger?.Log($"Erreur mise à jour progression: {ex.Message}", LogLevel.Warning);
            }
        }

        private string CalculateTimeRemaining(int percent, TimeSpan elapsed)
        {
            // ✅ MODIFIER: Afficher l'estimation au lieu des calculs complexes
            if (elapsed.TotalSeconds < 3)
            {
                var estimatedTotal = GetEstimatedDuration();
                return $"Estimation: {FormatTimeSpan(estimatedTotal)}";
            }

            try
            {
                var originalEstimate = GetEstimatedDuration();
                var remainingTime = originalEstimate - elapsed;

                // Si on dépasse l'estimation, recalculer simplement
                if (remainingTime.TotalSeconds <= 0 && percent > 10)
                {
                    double totalEstimated = (elapsed.TotalSeconds * 100) / Math.Max(percent, 5);
                    remainingTime = TimeSpan.FromSeconds(Math.Max(5, totalEstimated - elapsed.TotalSeconds));
                }
                else if (remainingTime.TotalSeconds <= 0)
                {
                    remainingTime = TimeSpan.FromSeconds(10);
                }

                return $"Écoulé: {FormatTimeSpan(elapsed)} | Restant: ~{FormatTimeSpan(remainingTime)}";
            }
            catch
            {
                var estimatedTotal = GetEstimatedDuration();
                return $"Écoulé: {FormatTimeSpan(elapsed)} | Estimation: {FormatTimeSpan(estimatedTotal)}";
            }
        }

        private void UpdateProgressBarValue(int value)
        {
            if (_progressBar != null && _progressBar.Style == ProgressBarStyle.Blocks)
            {
                _progressBar.Value = Math.Max(0, Math.Min(100, value));
            }
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalMinutes < 1)
                return $"{timeSpan.Seconds}s";
            else if (timeSpan.TotalHours < 1)
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            else
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
        }        

        public void SetMarqueeMode(bool active)
        {
            if (_progressBar?.InvokeRequired == true)
            {
                _progressBar.Invoke(new Action<bool>(SetMarqueeMode), active);
                return;
            }

            if (_progressBar != null)
            {
                if (active)
                {
                    _progressBar.Style = ProgressBarStyle.Marquee;
                    _progressBar.MarqueeAnimationSpeed = 30;
                }
                else
                {
                    _progressBar.Style = ProgressBarStyle.Blocks;
                    _progressBar.MarqueeAnimationSpeed = 0;
                }
            }
        }

        public void Reset()
        {
            if (_progressBar?.InvokeRequired == true)
            {
                _progressBar.Invoke(new Action(Reset));
                return;
            }

            if (_progressBar != null)
            {
                _progressBar.Value = 0;
                _progressBar.Style = ProgressBarStyle.Blocks;
                _progressBar.MarqueeAnimationSpeed = 0;
            }

            if (_statusLabel != null)
            {
                _statusLabel.Text = "Prêt";
            }
        }

        public void Complete()
        {
            if (_progressBar?.InvokeRequired == true)
            {
                _progressBar.Invoke(new Action(Complete));
                return;
            }

            _smoothingTimer?.Stop();

            if (_progressBar != null)
            {
                _progressBar.Value = 100;
                _progressBar.Style = ProgressBarStyle.Blocks;
            }

            if (_statusLabel != null)
            {
                var totalTime = DateTime.Now - _startTime;
                _statusLabel.Text = $"✅ Conversion terminée en {FormatTimeSpan(totalTime)}";
                _statusLabel.ForeColor = Color.DarkGreen;
            }
        }
    }
}