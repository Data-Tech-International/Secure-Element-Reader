using ReactiveUI;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.ViewModels.Interfaces;
using SecureElementReader.App.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;
using PCSC;
using PCSC.Monitoring;
using PCSC.Reactive;
using PCSC.Reactive.Events;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using SecureElementReader.App.Views;
using SecureElementReader.App.Models;
using System.Runtime.InteropServices;

namespace SecureElementReader.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel    
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _subscription;

        public Reactive.Bindings.ReactiveCollection<string> Readers { get; }
        public Reactive.Bindings.ReactiveCommand RefreshReaderListCommand { get; }       

        private readonly IDialogService dialogService;
        private readonly ICardReaderService cardReaderService;
        private readonly IApplicationDispatcher applicationDispatcher;
        private readonly IMainWindowProvider mainWindowProvider;
        private readonly ITaxCoreApiProxy taxCoreApiProxy;

        public string WelcomeMessage => Properties.Resources.Welcome;

        public IMenuViewModel MenuViewModel { get; }
        public ICertDetailsViewModel CertDetailsViewModel { get; }
        public ITopLanguageViewModel LanguageViewModel { get; }


        [Reactive]
        public string CardReaderName { get; set; }        

        [Reactive]
        public bool IsEnabled { get; set; }

        public ICommand CertDetailsCommand { get; }
        public ICommand VerifyPinCommand { get; }

        public MainWindowViewModel(IDialogService dialogService, 
            ICardReaderService cardReaderService,
            IMenuViewModel menuViewModel,
            ICertDetailsViewModel certDetailsViewModel,
            ITopLanguageViewModel languageViewModel,
            IMonitorFactory monitorFactory,
            IApplicationDispatcher applicationDispatcher,
            IMainWindowProvider mainWindowProvider,
            ITaxCoreApiProxy taxCoreApiProxy)
        {
            this.dialogService = dialogService;
            this.cardReaderService = cardReaderService;
            this.applicationDispatcher = applicationDispatcher;
            this.mainWindowProvider = mainWindowProvider;
            this.taxCoreApiProxy = taxCoreApiProxy;

            CertDetailsCommand = ReactiveCommand.Create(GetCertDetails);
            VerifyPinCommand = ReactiveCommand.CreateFromTask(ShowVerifyPinDialog);
            MenuViewModel = menuViewModel;
            CertDetailsViewModel = certDetailsViewModel;
            LanguageViewModel = languageViewModel;

            Readers = new Reactive.Bindings.ReactiveCollection<string>().AddTo(_disposables);
            RefreshReaderListCommand = new Reactive.Bindings.ReactiveCommand().AddTo(_disposables);

            RefreshReaderListCommand
                .Do(_=> applicationDispatcher.Dispatch(() => ShowLoadingOverlay(this)))
                .Select(_ => GetReaders())
                .Do(UpdateReaderList)
                .Do(readerNames => SubscribeToReaderEvents(monitorFactory, readerNames))
                .Subscribe()
                .AddTo(_disposables);

            //RefreshReaderListCommand.Execute();
        }
        
        private void UpdateReaderList(IEnumerable<string> readerNames)
        {
            Readers.ClearOnScheduler();            
            Readers.AddRangeOnScheduler(readerNames);            
        }

        private void SubscribeToReaderEvents(IMonitorFactory monitorFactory, IReadOnlyCollection<string> readerNames)
        {
            _subscription?.Dispose();

            if (readerNames.Count <= 0)
            {
                return;
            }

            _subscription = monitorFactory
                .CreateObservable(SCardScope.User, readerNames)                
                .Subscribe(
                    onNext: OnEvent,
                    onError: OnError);
        }

        private void OnError(Exception obj)
        {
            //log error
        }

        private void OnEvent(MonitorEvent obj)
        {   
            if (string.Equals(obj.GetType().Name, "CardRemoved"))
            {
                CertDetailsViewModel.ClearForm();                
            }
            else if (string.Equals(obj.GetType().Name, "MonitorInitialized"))
            {
                _ = GetCertDetails();
            }
            else if (string.Equals(obj.GetType().Name, "CardInserted"))
            {
               GetReaders();
               _ = GetCertDetails();   
            }           
        }

        private string[] GetReaders()
        {
            var readers = cardReaderService.LoadReaders().ToArray();
            if (readers != null && readers.Length > 0)
            {
                CardReaderName = readers[0];
                IsEnabled = true;
            }
            else
            {
                CardReaderName = Properties.Resources.NoCardReadesFounded;
                applicationDispatcher.Dispatch(() => HideLoadingOverlay());
                CertDetailsViewModel.ClearForm();
                IsEnabled = false;
            }

            return readers ?? Array.Empty<string>();
        }

        private async Task GetCertDetails()
        {
            applicationDispatcher.Dispatch(() => ShowLoadingOverlay(this));

            var details = cardReaderService.GetCertDetails();
            if (details.ErrorCodes.Count > 0)
            {
                await applicationDispatcher.DispatchAsync(() => ShowMessage(details.ErrorCodes));   
            }
            else
            {
                CertDetailsViewModel.CertDetailsModel = details;
                CertDetailsViewModel.SetVerifyFields();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    taxCoreApiProxy.Configure(CertDetailsViewModel.CertDetailsModel.UniqueIdentifier, CertDetailsViewModel.CertDetailsModel.CommonName, CertDetailsViewModel.CertDetailsModel.ApiUrl);
                    var internalDataStatus = await SubmitInternalData();
                    var commandsStatus = await ProcessPendingCommands();
                    CertDetailsViewModel.SetStatusFileds(internalDataStatus, commandsStatus);
                }
            }

            applicationDispatcher.Dispatch(() => HideLoadingOverlay());            
        }

        private void HideLoadingOverlay()
        {
            var mainWindow = (MainWindow)mainWindowProvider.GetMainWindow();
            mainWindow.HideLoadingOverlay();
        }

        private void ShowLoadingOverlay(object obj)
        {
            var mainWindow = (MainWindow)mainWindowProvider.GetMainWindow();
            mainWindow.ShowLoadingOverlay();
        }

        private async Task ShowMessage(List<string> errors)
        {
            var msg = MessageBoxManager.GetMessageBoxStandardWindow(
                    new MessageBoxStandardParams
                    {
                        ContentMessage = String.Join('\n', errors) + Environment.NewLine,
                        ContentHeader = Properties.Resources.Error,
                        ContentTitle = Properties.Resources.Error,
                        ShowInCenter = true,
                        Icon = MessageBoxAvaloniaEnums.Icon.Error,
                        Topmost = true,
                        SizeToContent = Avalonia.Controls.SizeToContent.WidthAndHeight,
                        WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner
                    });

           await msg.ShowDialog(mainWindowProvider.GetMainWindow());
        }

        private Task ShowVerifyPinDialog()
        {
            return dialogService.ShowDialogAsync(nameof(VerifyPinDialogViewModel));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _subscription?.Dispose();
            _disposables.Dispose();
        }

        private async Task<string> SubmitInternalData()
        {
            var internalData = cardReaderService.GetInternalData();
            var amountData = cardReaderService.GetAmountStatus();
            if (internalData != null && amountData != null)
            {
                var request = new SecureElementAuditRequest
                {
                    AuditData = internalData,
                    LimitData = amountData,
                };

                var response = await taxCoreApiProxy.SendInternalData(request);
               
                if (response)
                {
                    return "Success to submint internal data";
                }
                else
                {
                    return "Unable to submint internal data";
                }
            }
            else
            {                
                return "Can't read internal data form card";
            }
        }

        private async Task<string> ProcessPendingCommands()
        {
            var commands = await taxCoreApiProxy.GetPendingCommands();
            if (commands == null)
                return "Cant't get pending commands";

            if (commands.Count > 0)
            {                
                var commandStatus = cardReaderService.ProcessingCommand(commands);                
                
                if (commandStatus.Count > 0)
                {
                    var notifyCommandResult = await taxCoreApiProxy.CommandStatusUpdate(commandStatus);
                    if (ChechIsAllCommandExecutedSuccessfully(commands, commandStatus))
                    {   
                        if (CheckIsAllNotificationSentSuccessfuly(notifyCommandResult, commandStatus)) 
                            return "All commands executed successfully";
                        else
                            return "All commands executed but faild to notify TaxCore system";
                    }
                    else
                    {
                        return "Not all command executed successfully";
                    }
                }
                else
                {
                    return "Commands not executed";
                }                
            }
            else
            {
                return "There is no pending commands for this card";
            }
        }

        private bool CheckIsAllNotificationSentSuccessfuly(List<CommandsStatusResult> notifyCommandResult, List<CommandsStatusResult> commandStatus)
        {
            return notifyCommandResult.Where(s => s.Success).Count() == commandStatus.Where(s=>s.Success).Count();
        }

        private bool ChechIsAllCommandExecutedSuccessfully(List<Command> commands, List<CommandsStatusResult> commandsStatusResults) 
        {
            return commandsStatusResults.Where(s => s.Success).Count() == commands.Count();
        }
    }
}
