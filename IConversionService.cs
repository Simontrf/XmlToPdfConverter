using System;
using System.Threading;
using System.Threading.Tasks;

namespace XmlToPdfConverter.Core.Interfaces
{
    public class ConversionResult
    {
        public bool Success { get; set; }
        public string OutputPath { get; set; }
        public string ErrorMessage { get; set; }
        public TimeSpan Duration { get; set; }
        public long FileSizeBytes { get; set; }
    }

    public class ConversionProgress
    {
        public int Percentage { get; set; }
        public string CurrentStep { get; set; }
        public TimeSpan Elapsed { get; set; }
    }

    public interface IConversionService
    {
        Task<ConversionResult> ConvertAsync(
            string xmlPath,
            string xslPath,
            string outputPath,
            IProgress<ConversionProgress> progress = null);

        bool IsAvailable { get; }
        void Cleanup();
    }
}