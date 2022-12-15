using Moq;
using SecureElementReader.Interfaces;
using SecureElementReader.Models.Configurations;
using SecureElementReader.ViewModels;
using SecureElementReader.ViewModels.Services;
using Xunit;

namespace SecureElementReader.Test.ViewModels
{
    public class MenuViewModelTests
    {
        [Fact]
        public void TestAppClosing()
        {
            var applicationCloserMock = new Mock<IApplicationCloser>();
            var localizationServiceMock = new Mock<ILocalizationService>();
            var configurationMock = new Mock<SelectedLanguageConfiguration>();

            applicationCloserMock
                .Setup(m => m.CloseApp())
                .Verifiable();
            var dialogServiceMock = new Mock<IDialogService>();
            var menuViewModel = new MenuViewModel(applicationCloserMock.Object, dialogServiceMock.Object, localizationServiceMock.Object, configurationMock.Object);

            Assert.True(menuViewModel.ExitCommand.CanExecute(null));
            menuViewModel.ExitCommand.Execute(null);

            applicationCloserMock.Verify(m => m.CloseApp(), Times.Once());
        }

        [Fact]
        public void TestAboutDialogOpening()
        {
            var applicationCloserMock = new Mock<IApplicationCloser>();
            var dialogServiceMock = new Mock<IDialogService>();
            var localizationServiceMock = new Mock<ILocalizationService>();
            var configurationMock = new Mock<SelectedLanguageConfiguration>();
            dialogServiceMock
                .Setup(m => m.ShowDialogAsync(nameof(AboutDialogViewModel)))
                .Verifiable();
            var menuViewModel = new MenuViewModel(applicationCloserMock.Object, dialogServiceMock.Object, localizationServiceMock.Object, configurationMock.Object);

            Assert.True(menuViewModel.AboutCommand.CanExecute(null));
            menuViewModel.AboutCommand.Execute(null);

            dialogServiceMock.Verify(m => m.ShowDialogAsync(nameof(AboutDialogViewModel)), Times.Once());
        }
    }
}