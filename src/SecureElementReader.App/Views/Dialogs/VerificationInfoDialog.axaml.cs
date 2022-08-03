using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SecureElementReader.App.ViewModels.Interfaces;

namespace SecureElementReader.App.Views.Dialogs
{
    public partial class VerificationInfoDialog : DialogWindowBase
    {
        private IVerificationInfoDialog ViewModel => (IVerificationInfoDialog)DataContext;
        public VerificationInfoDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        protected override void OnOpened()
        {
            var details = ViewModel.VerificationDetails;

            if (!string.IsNullOrWhiteSpace(details))
            {
                var sp = this.FindControl<StackPanel>("Panel");
                sp.Children.Clear();
                foreach (var item in details.Split('\n'))
                {
                    if (item.StartsWith("Error") || item.StartsWith("Status"))
                    {
                        sp.Children.Add(new TextBlock
                        {
                            Text = item,
                            FontWeight = FontWeight.Bold,
                            TextWrapping = TextWrapping.Wrap
                        });
                    }
                    else
                    {
                        sp.Children.Add(new TextBlock
                        {
                            Text = item,
                            TextWrapping = TextWrapping.Wrap
                        });
                    }

                }
                sp.Children.Add(new TextBlock
                {
                    Text = "Possible solution:",
                    FontWeight = FontWeight.Bold,
                    TextWrapping = TextWrapping.Wrap
                });
                sp.Children.Add(new TextBox
                {
                    Text = $"Install RCA and ICA certificate from this url: {ViewModel.TapUrl}",
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true,
                    BorderThickness = new Thickness(0),
                    Background = Brushes.Transparent
                });
            }

            base.OnOpened();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
