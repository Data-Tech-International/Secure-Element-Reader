using Moq.AutoMock;
using SecureElementReader.ViewModels;
using Xunit;

namespace SecureElementReader.Test.ViewModels
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
