using SecureElementReader.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Interfaces
{
    public interface ITaxCoreApiProxy
    {
        bool SendInternalData(SecureElementAuditRequest request, string commonName, string apiUrl);
    }
}
