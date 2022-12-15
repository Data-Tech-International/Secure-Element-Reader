using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using PCSC;
using PCSC.Monitoring;
using PCSC.Reactive;
using PCSC.Reactive.Events;
using Reactive.Bindings.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SecureElementReader.Enums;
using SecureElementReader.Interfaces;
using SecureElementReader.Models;
using SecureElementReader.ViewModels.Interfaces;
using SecureElementReader.ViewModels.Services;
using SecureElementReader.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;

namespace SecureElementReader.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _subscription;

        public Reactive.Bindings.ReactiveCollection<string> Readers { get; }
        public Reactive.Bindings.ReactiveCommand RefreshReaderListCommand { get; }

        private readonly IDialogService _dialogService;
        private readonly ICardReaderService _cardReaderService;
        private readonly IApplicationDispatcher _applicationDispatcher;
        private readonly IMainWindowProvider _mainWindowProvider;
        private readonly ITaxCoreApiProxy _taxCoreApiProxy;

        public IMenuViewModel MenuViewModel { get; }
        public ICertDetailsViewModel CertDetailsViewModel { get; }


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
            IMonitorFactory monitorFactory,
            IApplicationDispatcher applicationDispatcher,
            IMainWindowProvider mainWindowProvider,
            ITaxCoreApiProxy taxCoreApiProxy)
        {
            _dialogService = dialogService;
            _cardReaderService = cardReaderService;
            _applicationDispatcher = applicationDispatcher;
            _mainWindowProvider = mainWindowProvider;
            _taxCoreApiProxy = taxCoreApiProxy;

            CertDetailsCommand = ReactiveCommand.Create(GetCertDetails);
            VerifyPinCommand = ReactiveCommand.CreateFromTask(ShowVerifyPinDialog);
            MenuViewModel = menuViewModel;
            CertDetailsViewModel = certDetailsViewModel;


            Readers = new Reactive.Bindings.ReactiveCollection<string>().AddTo(_disposables);
            RefreshReaderListCommand = new Reactive.Bindings.ReactiveCommand().AddTo(_disposables);

            RefreshReaderListCommand
                .Do(_ => applicationDispatcher.Dispatch(() => ShowLoadingOverlay(this)))
                .Select(_ => GetReaders())
                .Do(UpdateReaderList)
                .Do(readerNames => SubscribeToReaderEvents(monitorFactory, readerNames))
                .Subscribe()
                .AddTo(_disposables);
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
            if (Equals(obj.GetType().Name, "CardRemoved"))
            {
                CertDetailsViewModel.ClearForm();
            }
            else if (Equals(obj.GetType().Name, "MonitorInitialized"))
            {
                _ = GetCertDetails();
            }
            else if (Equals(obj.GetType().Name, "CardInserted"))
            {
                GetReaders();
                _ = GetCertDetails();
            }
            else if (Equals(obj.GetType().Name, "CardStatusChanged"))
            {
                GetReaders();
            }
        }

        private string[] GetReaders()
        {
            var readers = _cardReaderService.LoadReaders().ToArray();
            if (readers != null && readers.Length > 0)
            {
                CardReaderName = readers[0];
                IsEnabled = true;
            }
            else
            {
                CardReaderName = String.Empty;
                _applicationDispatcher.Dispatch(() => HideLoadingOverlay());
                CertDetailsViewModel.ClearForm();
                IsEnabled = false;
            }

            return readers ?? Array.Empty<string>();
        }

        private async Task GetCertDetails()
        {

            _applicationDispatcher.Dispatch(() => ShowLoadingOverlay(this));

            var details = _cardReaderService.GetCertDetails();
            if (details.ErrorCodes.Count > 0)
            {
                await _applicationDispatcher.DispatchAsync(() => ShowMessage(details.ErrorCodes));
            }
            else
            {
                CertDetailsViewModel.CertDetailsModel = details;
                CertDetailsViewModel.SetVerifyFields();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    _taxCoreApiProxy.Configure(CertDetailsViewModel.CertDetailsModel.UniqueIdentifier, CertDetailsViewModel.CertDetailsModel.CommonName, CertDetailsViewModel.CertDetailsModel.ApiUrl);
                    var internalDataStatus = await SubmitInternalData();
                    var commandsStatus = await ProcessPendingCommands();
                    CertDetailsViewModel.SetStatusFields(internalDataStatus, commandsStatus);
                }
            }
            _applicationDispatcher.Dispatch(() => HideLoadingOverlay());
        }

        private void HideLoadingOverlay()
        {
            var mainWindow = (MainWindow)_mainWindowProvider.GetMainWindow();
            mainWindow.HideLoadingOverlay();
        }

        private void ShowLoadingOverlay(object obj)
        {
            var mainWindow = (MainWindow)_mainWindowProvider.GetMainWindow();
            mainWindow.ShowLoadingOverlay();
        }

        private async Task ShowMessage(List<string> errors)
        {
            var msg = MessageBoxManager.GetMessageBoxStandardWindow(
                    new MessageBoxStandardParams
                    {
                        ContentMessage = String.Join('\n', errors) + Environment.NewLine,
                        ShowInCenter = true,
                        Icon = MessageBoxAvaloniaEnums.Icon.Error,
                        Topmost = true,
                        SizeToContent = Avalonia.Controls.SizeToContent.WidthAndHeight,
                        WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner
                    });

            await msg.ShowDialog(_mainWindowProvider.GetMainWindow());
        }

        private Task ShowVerifyPinDialog()
        {
            return _dialogService.ShowDialogAsync(nameof(VerifyPinDialogViewModel));
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
            var internalData = _cardReaderService.GetInternalData();
            var amountData = _cardReaderService.GetAmountStatus();

            if (internalData != null && amountData != null)
            {
                var request = new SecureElementAuditRequest
                {
                    AuditData = internalData,
                    LimitData = amountData,
                };

                var response = await _taxCoreApiProxy.SendInternalData(request);

                if (response)
                {
                    return SubmitMessages.SuccessSubmit.ToString();
                }
                else
                {
                    return SubmitMessages.UnableToSubmit.ToString();
                }
            }
            else
            {
                return SubmitMessages.CantReadInternal.ToString();
            }
        }

        private async Task<string> ProcessPendingCommands()
        {
            var commands = await _taxCoreApiProxy.GetPendingCommands();
            if (commands == null)
                return CommandsMessages.CannotGetPendingCommands.ToString();

            if (commands.Count > 0)
            {
                var commandStatus = _cardReaderService.ProcessingCommand(commands);

                if (commandStatus.Count > 0)
                {
                    var notifyCommandResult = await _taxCoreApiProxy.CommandStatusUpdate(commandStatus);
                    if (ChechIsAllCommandExecutedSuccessfully(commands, commandStatus))
                    {
                        if (CheckIsAllNotificationSentSuccessfuly(notifyCommandResult, commandStatus))
                        {
                            return CommandsMessages.AllCommandsExecutedSuccessfully.ToString();
                        }
                        else
                        {
                            return CommandsMessages.AllCommandsExecutedButFailedToNotifyTaxCoreSystem.ToString();
                        }
                    }
                    else
                    {
                        return CommandsMessages.NotAllCommandExecutedSuccessfully.ToString();
                    }
                }
                else
                {
                    return CommandsMessages.CommandsNotExecuted.ToString();
                }
            }
            else
            {
                return CommandsMessages.ThereIsNoPendingCommandsForThisCard.ToString();
            }
        }


        private bool CheckIsAllNotificationSentSuccessfuly(List<CommandsStatusResult> notifyCommandResult, List<CommandsStatusResult> commandStatus)
        {
            return notifyCommandResult.Where(s => s.Success).Count() == commandStatus.Where(s => s.Success).Count();
        }

        private bool ChechIsAllCommandExecutedSuccessfully(List<Command> commands, List<CommandsStatusResult> commandsStatusResults)
        {
            return commandsStatusResults.Where(s => s.Success).Count() == commands.Count();
        }
    }
}
