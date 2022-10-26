using SecureElementReader.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.ViewModels.Interfaces
{
    public interface IMenuViewModel
    {
        public void Translate(string targetLanguage);

        public void StartUpTranslate();
    }
}
