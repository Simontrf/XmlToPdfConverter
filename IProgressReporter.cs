using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IProgressReporter
    {
        void Report(int percent, string message);
    }

}
