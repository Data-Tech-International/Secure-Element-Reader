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
        PinVerify = 0x11,

        /// <summary>
        /// EXPORT INTERNAL DATA – exports encrypted Internal Data structure
        /// </summary>
        /// <remarks>Fiscalization</remarks>
        ExportInternalData = 0x12,

        /// <summary>
        /// AMOUNT STATUS – 16 bytes long data structure ( 8 bytes for sum SALE and REFUND, and 8 bytes for MAXIMAL sum amount )
        /// </summary>
        /// <remarks>Fiscalization</remarks>
        AmountStatus = 0x14,
    }
}
