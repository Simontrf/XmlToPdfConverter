using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IXmlToPdfConverter
    {
        bool Convert(string xmlPath, string originalXmlPath, IProgressReporter progress, ILogger logger);
    }

}
