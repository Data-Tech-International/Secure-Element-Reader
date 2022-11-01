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
                App.Current.TryFindResource("PossibleSolution", out var possibleSolution);
                App.Current.TryFindResource("InstallRCAandICAurl", out var installRCAandICAurl);
                App.Current.TryFindResource("InstallRCAandICA", out var installRCAandICA);

                var sp = this.FindControl<StackPanel>("Panel");
                sp.Children.Clear();
                foreach (var item in details.Split('\n'))
                {
                    if (item.StartsWith("Error".Split(" ")[0]) || 
                        item.StartsWith("Status"))
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
                    Text = $"{possibleSolution}:",
                    FontWeight = FontWeight.Bold,
                    TextWrapping = TextWrapping.Wrap
                });
                sp.Children.Add(new TextBlock
                {
                    Text = $"{installRCAandICAurl}:",
                    TextWrapping = TextWrapping.Wrap
                });
                sp.Children.Add(new Button
                {
                    Content = $"{installRCAandICA}",
                    Command = ViewModel.GoToTap
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
