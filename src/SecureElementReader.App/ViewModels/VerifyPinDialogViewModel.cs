using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using SecureElementReader.App.ViewModels.Implementations.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;

namespace SecureElementReader.App.ViewModels
{
    public class VerifyPinDialogViewModel : DialogViewModelBase
    {
        private readonly ICardReaderService _cardReaderService;
        private readonly IMainWindowProvider _mainWindowProvider;

        public ICommand CloseButton { get; }

        [Reactive]
        public string Pin { get; set; }

        [Reactive]
        public string SeResult { get; set; }

        [Reactive]
        public string PkiResult { get; set; }

        [Reactive]
        public string TaxMessage { get; set; }
        
        [Reactive]
        public bool PleaseInsertPin { get; set; }

        [Reactive]
        public bool PinMustBe4Char { get; set; }

        [Reactive]
        public bool PkiPinOk { get; set; }

        [Reactive]
        public bool SePinOk { get; set; }


        [Reactive]
        public bool SeAppletLocked { get; set; }

        [Reactive]
        public bool SePinWrong { get; set; }

        [Reactive]
        public bool PkiAppletLocked { get; set; }

        [Reactive]
        public string TrysLeft { get; set; }

        [Reactive]
        public bool WrongPinAlertText { get; set; }

        [Reactive]
        public Avalonia.Media.IBrush SeResultColor { get; set; }

        [Reactive]
        public Avalonia.Media.IBrush PkiResultColor { get; set; }

        [Reactive]
        public bool ShowTaxMessage { get; set; }

        public ICommand VerifyCommand { get; set; }

        public VerifyPinDialogViewModel(ICardReaderService cardReaderService, IMainWindowProvider mainWindowProvider)
        {
            this._cardReaderService = cardReaderService; 
            this._mainWindowProvider = mainWindowProvider;
            CloseButton = ReactiveCommand.Create(ButtonClose);
            VerifyCommand = ReactiveCommand.Create(VerifyPin);           
        }

        private void VerifyPin()
        {
            if (string.IsNullOrWhiteSpace(Pin))
            {
                PleaseInsertPin = true;
                PinMustBe4Char = false;
                PkiPinOk = false;
                SePinOk = false;
                WrongPinAlertText = false;

            }
            else if (Pin.Length == 4)
            {             
                PinMustBe4Char = true;
                PleaseInsertPin = false;
                PkiPinOk = false;
                SePinOk = false;
                WrongPinAlertText = false;

            }
            else
            {
                var result = _cardReaderService.VerifyPin(Pin);
                if (result.ErrorList.Any())
                {
                    ShowErrorMsg(result.ErrorList);                    
                }
                else
                {
                    ValidateResult(result);                    
                }
            }
        }

        private void ShowErrorMsg(List<string> errorList)
        {
            var msg = MessageBoxManager.GetMessageBoxStandardWindow(
                    new MessageBoxStandardParams
                    {
                        ContentMessage = String.Join('\n', errorList) + Environment.NewLine,
                        ContentHeader = _mainWindowProvider.GetMainWindow().GetResourceObservable("Error").ToString(),
                        ContentTitle = _mainWindowProvider.GetMainWindow().GetResourceObservable("Error").ToString(),
                        ShowInCenter = true,
                        Icon = MessageBoxAvaloniaEnums.Icon.Error,
                        Topmost = true,
                        WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner,
                        SizeToContent = Avalonia.Controls.SizeToContent.WidthAndHeight
                    });
            msg.ShowDialog(_mainWindowProvider.GetMainWindow());
        }

        private void ValidateResult(VerifyPinModel result)
        {
            if (result.PkiPinSuccess)
            {
                PkiPinOk = true;
                PinMustBe4Char = false;
                PleaseInsertPin = false;
                WrongPinAlertText = false;
            }
            else
            {
                if (result.PkiAppletLocked)
                {
                    PkiAppletLocked = true;
                    PinMustBe4Char = false;
                    PleaseInsertPin = false;
                    PkiPinOk = false;
                    SePinOk = false;

                }
                else
                {
                    TrysLeft = (result.PkiTrysLeft - 192).ToString();
                    WrongPinAlertText = true;
                    PinMustBe4Char = false;
                    PleaseInsertPin = false;
                    SePinOk = false;
                    PkiPinOk = false;
                }
            }

            if (result.SePinSuccess)
            {
                SePinOk = true;
                PinMustBe4Char = false;
                PleaseInsertPin = false;
                WrongPinAlertText = false;
            }
            else
            {
                if (result.SeAppletLocked)
                {
                    SeAppletLocked = true;
                    PinMustBe4Char = false;
                    PleaseInsertPin = false;
                    WrongPinAlertText = false;

                }
                else
                {
                    SePinWrong = true;
                    PinMustBe4Char = false;
                    PleaseInsertPin = false;
                }                
            }

            if (result.SeAppletLocked & result.PkiAppletLocked)
            {

                ShowTaxMessage = true;
                TaxMessage = _mainWindowProvider.GetMainWindow().GetResourceObservable("ReturnCard").ToString();
            }
            else if(result.SeAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = _mainWindowProvider.GetMainWindow().GetResourceObservable("SeAppletLocked").ToString();
            }
            else if(result.PkiAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = _mainWindowProvider.GetMainWindow().GetResourceObservable("PkiAppletLocked").ToString();
            }

        }
        private void ButtonClose()
        {
            Close();
        }
    }
}
