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
                    if (item.StartsWith(Properties.Resources.ErrorAtDepth.Split(" ")[0]) || 
                        item.StartsWith(Properties.Resources.Status))
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
                    Text = $"{Properties.Resources.PossibleSolution}:",
                    FontWeight = FontWeight.Bold,
                    TextWrapping = TextWrapping.Wrap
                });
                sp.Children.Add(new TextBlock
                {
                    Text = $"{Properties.Resources.InstallRCAandICAurl}:",
                    TextWrapping = TextWrapping.Wrap
                });
                sp.Children.Add(new Button
                {
                    Content = $"{Properties.Resources.InstallRCAandICA}",
                    Command = ViewModel.GoToTAP
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
