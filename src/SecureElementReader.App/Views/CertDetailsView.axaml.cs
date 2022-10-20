using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace SecureElementReader.App.Views
{
    public partial class CertDetailsView : UserControl
    {
        readonly TextBox whitePointTextBox;

        public CertDetailsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.whitePointTextBox = this.FindControl<TextBox>(nameof(whitePointTextBox));
        }

        
    }
}
