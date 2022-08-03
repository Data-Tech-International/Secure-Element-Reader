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

        [Reactive]
        public string Pin { get; set; }

        [Reactive]
        public string SeResult { get; set; }

        [Reactive]
        public string PkiResult { get; set; }

        [Reactive]
        public string TaxMessage { get; set; }

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

            VerifyCommand = ReactiveCommand.Create(VerifyPin);           
        }

        private void VerifyPin()
        {
            if (string.IsNullOrWhiteSpace(Pin))
            {
                SeResult = Properties.Resources.PleaseInsertPIN;
                SeResultColor = Avalonia.Media.Brushes.Red;
                PkiResult = String.Empty;
            }
            else if (Pin.Length < 4 || Pin.Length > 4)
            {
                
                SeResult = Properties.Resources.PinMustBe4char;
                SeResultColor = Avalonia.Media.Brushes.Red;
                PkiResult = String.Empty;
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
                        ContentMessage = String.Join('\n', errorList),
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
                PkiResult = Properties.Resources.PkiPinOK;
                PkiResultColor = Avalonia.Media.Brushes.Green;
            }
            else
            {
                if (result.PKIAppletLocked)
                {
                    PkiResult = Properties.Resources.PkiAppletLocked;
                }
                else
                {
                    PkiResult =  Properties.Resources.PkiPinWrong.Replace("[xxx]", (result.PkiTrysLeft - 192).ToString());
                }                
                PkiResultColor = Avalonia.Media.Brushes.Red;
            }

            if (result.SePinSuccess)
            {
                SeResult = Properties.Resources.SePinOK;
                SeResultColor = Avalonia.Media.Brushes.Green;
            }
            else
            {
                if (result.SEAppletLocked)
                {
                    SeResult = Properties.Resources.SeAppletLocked;
                }
                else
                {
                    SeResult = Properties.Resources.SePinWrong;
                }                
                SeResultColor = Avalonia.Media.Brushes.Red;
            }

            if (result.SEAppletLocked & result.PKIAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = Properties.Resources.ReturnCard;
            }
            else if(result.SEAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = "SE applet is locked, please return the card.";
            }
            else if(result.PKIAppletLocked)
            {
                ShowTaxMessage = true;
                TaxMessage = "PKI applet is locked, please return the card.";
            }

        }
    }
}
