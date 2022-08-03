using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using SecureElementReader.App.ViewModels.Interfaces;
using System;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;

namespace SecureElementReader.App.Views
{
    public partial class TopLanguageView : UserControl
    {
        private ITopLanguageViewModel topLanguageViewModel => (ITopLanguageViewModel)DataContext;

        public TopLanguageView()
        {
            
            InitializeComponent();            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var selectedLng = (LanguageModel)e.AddedItems[0];
            if (selectedLng.Code != topLanguageViewModel.CurrentLanguage.Code)
            {

                ShowMessage();
                topLanguageViewModel.CurrentLanguage = selectedLng;
                topLanguageViewModel.SaveChanges();
                
            }            
        }

        private void ShowMessage()
        {
            var msg = MessageBoxManager.GetMessageBoxStandardWindow(
                   new MessageBoxStandardParams
                   {
                       ContentMessage = Properties.Resources.ChangeLang,
                       ContentHeader = Properties.Resources.Info,
                       ContentTitle = Properties.Resources.Info,
                       ShowInCenter = true,
                       Icon = MessageBoxAvaloniaEnums.Icon.None,
                       Topmost = true,
                       WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner,
                       SizeToContent = Avalonia.Controls.SizeToContent.WidthAndHeight
                   });         
            msg.ShowDialog(topLanguageViewModel.GetMainWindow());
        }
    }
}
