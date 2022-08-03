using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Enums
{
    public enum ApduClasses : byte
    {
        /// <summary>
        /// The select
        /// </summary>
        SelectApp = 0x00,

        /// <summary>
        /// The command
        /// </summary>
        SelectCommand = 0x88
    }
}
