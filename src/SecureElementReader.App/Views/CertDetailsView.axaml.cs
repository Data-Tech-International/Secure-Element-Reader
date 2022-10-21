using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using SecureElementReader.App.Enums;
using SecureElementReader.App.ViewModels;
using System;
using System.Threading.Tasks;

namespace SecureElementReader.App.Views
{
    public partial class CertDetailsView : UserControl
    {
        private Label lblCommandsMsg => this.FindControl<Label>("lblCommnads");
        private Label lblAuditMsg => this.FindControl<Label>("lblAudit");

        public CertDetailsView()
        {
            InitializeComponent();

            WaithForViewModel();
        }

        private async Task WaithForViewModel()
        {
            Window hostWindow = (Window)this.VisualRoot;

            while (hostWindow == null)
            {
                hostWindow = (Window)this.VisualRoot;
                var vm = (CertDetailsViewModel)DataContext;
                if (vm != null)
                {
                    vm.SetStaus += SetStatus_Event;
                    vm.ClearFields += ClearFields_Event;
                }
                await Task.Delay(50);
            }
        }

        private void ClearFields_Event()
        {
            Dispatcher.UIThread.Post(() => { lblCommandsMsg.Content = string.Empty; });
        }

        private void SetStatus_Event(string internalDataStatus, string commandsStatus)
        {
            switch (Enum.Parse(typeof(CommandsMessages), commandsStatus))
            {
                case CommandsMessages.CannotGetPendingCommands:
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("SuccessSubmit")); });                    
                    break;
                case CommandsMessages.AllCommandsExecutedSuccessfully: 
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("UnableToSubmit")); });
                    break;
                default:
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
