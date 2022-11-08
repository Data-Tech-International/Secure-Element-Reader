using System.Collections.Generic;

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
        public bool SeAppletLocked { get; internal set; }
        public bool PkiAppletLocked { get; internal set; }
        public List<string> ErrorList { get; set; }
    }
}
