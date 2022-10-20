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
        private readonly ICardReaderService cardReaderService;
        private readonly IMainWindowProvider mainWindowProvider;

        public ICommand CloseButton { get; }

        [Reactive]
        public string Pin { get; set; }

        [Reactive]
        public string SeResult { get; set; }

        [Reactive]
        public string PkiResult { get; set; }

        [Reactive]
        public string TaxMessage { get; set; }

        //MARIO
        [Reactive]
        public bool PleaseInsertPIN { get; set; }

        [Reactive]
        public bool PinMustBe4char { get; set; }

        [Reactive]
        public bool PkiPinOK { get; set; }

        [Reactive]
        public bool SePinOK { get; set; }


        [Reactive]
        public bool SEAppletLocked { get; set; }

        [Reactive]
        public bool SePinWrong { get; set; }

        [Reactive]
        public bool PkiAppletLocked { get; set; }

        [Reactive]
        public string TrysLeft { get; set; }

        [Reactive]
        public bool WrongPinAlertText { get; set; }



        //OVDE SAMO PREKIDACI
        [Reactive]
        public Avalonia.Media.IBrush SeResultColor { get; set; }

        [Reactive]
        public Avalonia.Media.IBrush PkiResultColor { get; set; }

        [Reactive]
        public bool ShowTaxMessage { get; set; }

        public ICommand VerifyCommand { get; set; }

        public VerifyPinDialogViewModel(ICardReaderService cardReaderService, IMainWindowProvider mainWindowProvider)
        {
            this.cardReaderService = cardReaderService; 
            this.mainWindowProvider = mainWindowProvider;
            CloseButton = ReactiveCommand.Create(ButtonClose);
            VerifyCommand = ReactiveCommand.Create(VerifyPin);           
        }

        private void VerifyPin()
        {
            if (string.IsNullOrWhiteSpace(Pin))
            {
                PleaseInsertPIN = true;
                PinMustBe4char = false;
                PkiPinOK = false;
                SePinOK = false;
                WrongPinAlertText = false;

            }
            else if (Pin.Length < 4 || Pin.Length > 4)
            {             
                PinMustBe4char = true;
                PleaseInsertPIN = false;
                PkiPinOK = false;
                SePinOK = false;
                WrongPinAlertText = false;

            }
            else
            {
                var result = cardReaderService.VerifyPin(Pin);
                if (result.ErrorList.Count() > 0)
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
                        ContentHeader = Properties.Resources.Error,
                        ContentTitle = Properties.Resources.Error,
                        ShowInCenter = true,
                        Icon = MessageBoxAvaloniaEnums.Icon.Error,
                        Topmost = true,
                        WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner,
                        SizeToContent = Avalonia.Controls.SizeToContent.WidthAndHeight
                    });
            msg.ShowDialog(mainWindowProvider.GetMainWindow());
        }

        private void ValidateResult(VerifyPinModel result)
        {
            if (result.PkiPinSuccess)
            {
                PkiPinOK = true;
                PinMustBe4char = false;
                PleaseInsertPIN = false;
                WrongPinAlertText = false;
            }
            else
            {
                if (result.PKIAppletLocked)
                {
                    PkiAppletLocked = true;
                    PinMustBe4char = false;
                    PleaseInsertPIN = false;
                    PkiPinOK = false;
                    SePinOK = false;

                }
                else
                {
                    TrysLeft = (result.PkiTrysLeft - 192).ToString();
                    WrongPinAlertText = true;
                    PinMustBe4char = false;
                    PleaseInsertPIN = false;
                    SePinOK = false;
                    PkiPinOK = false;
                }
            }

            if (result.SePinSuccess)
            {
                SePinOK = true;
                PinMustBe4char = false;
                PleaseInsertPIN = false;
                WrongPinAlertText = false;
            }
            else
            {
                if (result.SEAppletLocked)
                {
                    SEAppletLocked = true;
                    PinMustBe4char = false;
                    PleaseInsertPIN = false;
                    WrongPinAlertText = false;

                }
                else
                {
                    SePinWrong = true;
                    PinMustBe4char = false;
                    PleaseInsertPIN = false;
                }                
            }

            if (result.SEAppletLocked & result.PKIAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = Properties.Resources.ReturnCard;//TaxMessageSE$PKI locked = vrati karticu u poresku
            }
            else if(result.SEAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = "SE applet is locked, please return the card.";//TaxMessageSE locked = true;
            }
            else if(result.PKIAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = "PKI applet is locked, please return the card.";//TaxMessagePKI locked = true;
            }

        }
        private void ButtonClose()
        {
            Close();
        }
    }
}
