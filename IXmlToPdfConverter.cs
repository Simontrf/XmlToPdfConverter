using System.Threading.Tasks;

namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IXmlToPdfConverter
    {
        Task<bool> Convert(string xmlPath, string outputPdfPath, IProgressReporter progress, ILogger logger);
    }

}
