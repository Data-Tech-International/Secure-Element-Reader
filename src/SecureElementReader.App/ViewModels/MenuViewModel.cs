using ReactiveUI;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.ViewModels.Interfaces;
using SecureElementReader.App.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureElementReader.App.ViewModels
{
    public class MenuViewModel : ViewModelBase, IMenuViewModel
    {
        private readonly IDialogService dialogService;

        public ICommand ExitCommand { get; }

        public ICommand AboutCommand { get; }

        public MenuViewModel(
            IApplicationCloser applicationCloser,
            IDialogService dialogService)
        {
            this.dialogService = dialogService;

            ExitCommand = ReactiveCommand.Create(applicationCloser.CloseApp);
            AboutCommand = ReactiveCommand.CreateFromTask(ShowAboutDialogAsync);
        }

        private Task ShowAboutDialogAsync()
        {
            return dialogService.ShowDialogAsync(nameof(AboutDialogViewModel));
        }
    }
}
