using Moq.AutoMock;
using SecureElementReader.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecureElementReader.App.Test.ViewModels
{
    public class AboutDialogViewModelTests
    {
        private readonly AutoMocker _autoMocker;

        public AboutDialogViewModelTests()
        {
            _autoMocker = new AutoMocker();
        }

        [Fact]
        public void TestInformation()
        {
            const string version = "1.0.0";

            var dialog = _autoMocker.CreateInstance<AboutDialogViewModel>();

            Assert.Equal(version, dialog.AssemblyVersion);
        }
    }
}
