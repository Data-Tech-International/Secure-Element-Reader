using Moq;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.ViewModels;
using SecureElementReader.App.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecureElementReader.App.Test.ViewModels
{
    public class MenuViewModelTests
    {
        [Fact]
        public void TestAppClosing()
        {
            var applicationCloserMock = new Mock<IApplicationCloser>();
            applicationCloserMock
                .Setup(m => m.CloseApp())
                .Verifiable();
            var dialogServiceMock = new Mock<IDialogService>();
            var menuViewModel = new MenuViewModel(applicationCloserMock.Object, dialogServiceMock.Object);

            Assert.True(menuViewModel.ExitCommand.CanExecute(null));
            menuViewModel.ExitCommand.Execute(null);

            applicationCloserMock.Verify(m => m.CloseApp(), Times.Once());
        }

        [Fact]
        public void TestAboutDialogOpening()
        {
            var applicationCloserMock = new Mock<IApplicationCloser>();
            var dialogServiceMock = new Mock<IDialogService>();
            dialogServiceMock
                .Setup(m => m.ShowDialogAsync(nameof(AboutDialogViewModel)))
                .Verifiable();
            var menuViewModel = new MenuViewModel(applicationCloserMock.Object, dialogServiceMock.Object);

            Assert.True(menuViewModel.AboutCommand.CanExecute(null));
            menuViewModel.AboutCommand.Execute(null);

            dialogServiceMock.Verify(m => m.ShowDialogAsync(nameof(AboutDialogViewModel)), Times.Once());
        }        
    }
}
