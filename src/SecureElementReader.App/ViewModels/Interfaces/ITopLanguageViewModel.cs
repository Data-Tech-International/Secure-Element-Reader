using Avalonia.Controls;
using SecureElementReader.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.ViewModels.Interfaces
{
    public interface ITopLanguageViewModel
    {
        void SaveChanges();
        public LanguageModel CurrentLanguage { get; set; }

        Window GetMainWindow();
    }
}
