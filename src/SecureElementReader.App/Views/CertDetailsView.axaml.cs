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
            Dispatcher.UIThread.Post(() => { lblAuditMsg.Content = string.Empty; });
        }

        private void SetStatus_Event(string internalDataStatus, string commandsStatus)
        {
            switch (Enum.Parse(typeof(SubmitMessages), internalDataStatus))
            {
                case SubmitMessages.SuccessSubmit:
                    Dispatcher.UIThread.Post(() => { lblAuditMsg.Bind(Label.ContentProperty, this.GetResourceObservable("SuccessSubmit")); });
                    break;
                case SubmitMessages.UnableToSubmit:
                    Dispatcher.UIThread.Post(() => { lblAuditMsg.Bind(Label.ContentProperty, this.GetResourceObservable("UnableToSubmit")); });
                    break;
                case SubmitMessages.CantReadInternal:
                    Dispatcher.UIThread.Post(() => { lblAuditMsg.Bind(Label.ContentProperty, this.GetResourceObservable("CantReadInternal")); });
                    break;

                default:
                    break;
            }



            switch (Enum.Parse(typeof(CommandsMessages), commandsStatus))
            {
                case CommandsMessages.CannotGetPendingCommands:
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("CannotGetPendingCommands")); });                    
                    break;
                case CommandsMessages.AllCommandsExecutedSuccessfully: 
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("AllCommandsExecutedSuccessfully")); });
                    break;
                case CommandsMessages.AllCommandsExecutedButFailedToNotifyTaxCoreSystem:
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("AllCommandsExecutedButFailedToNotifyTaxCoreSystem")); });
                    break;
                case CommandsMessages.NotAllCommandEexecutedSuccessfully:
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("NotAllCommandEexecutedSuccessfully")); });
                    break;
                case CommandsMessages.CommandsNotExecuted:
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("CommandsNotExecuted")); });
                    break;
                case CommandsMessages.ThereIsNoPendingCommandsForThisCard:
                    Dispatcher.UIThread.Post(() => { lblCommandsMsg.Bind(Label.ContentProperty, this.GetResourceObservable("ThereIsNoPendingCommandsForThisCard")); });
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
