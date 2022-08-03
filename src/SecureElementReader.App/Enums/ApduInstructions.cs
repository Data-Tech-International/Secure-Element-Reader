using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Enums
{
    public enum ApduInstructions : byte
    {
        ExportCert = 0x04,
        PinVerify = 0x11
    }
}
