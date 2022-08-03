using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Models
{
    public class VerifyPinModel
    {
        public VerifyPinModel()
        {
            ErrorList = new List<string>();
        }
        public bool PkiPinSuccess { get; set; }
        public int PkiTrysLeft { get; set; }
        public bool SePinSuccess { get; set; }
        public int SeTrysLeft { get; set; }
        public bool SEAppletLocked { get; internal set; }
        public bool PKIAppletLocked { get; internal set; }
        public List<string> ErrorList { get; set; } 
    }
}
