using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Models
{
    public class SecureElementAuditRequest
    {
        public byte[] AuditData { get; set; }
        public byte[] LimitData { get; set; }
    }
}
