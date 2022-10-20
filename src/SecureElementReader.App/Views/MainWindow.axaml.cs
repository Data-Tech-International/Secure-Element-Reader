using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace SecureElementReader.App.Views
{
    partial class MainWindow : Window
    {
        private Grid overlayGrid => this.FindControl<Grid>("OverlayGrid");
        private Label lblLoading => this.FindControl<Label>("LblLoading");

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void ShowOverlay() => overlayGrid.ZIndex = 1000;
         
        public void HideOverlay() => overlayGrid.ZIndex = -1;



        public void ShowLoadingOverlay() 
        {
            overlayGrid.ZIndex = 1000;
            overlayGrid.Background = Brushes.Gray;
            overlayGrid.Opacity = 0.5;
            lblLoading.Opacity = 1;
            lblLoading.ZIndex = 1001;
        }

        public void HideLoadingOverlay() 
        {
            overlayGrid.ZIndex = -1;
            overlayGrid.Background = null;
            overlayGrid.Opacity = 0;
            lblLoading.Opacity = 0;
            lblLoading.ZIndex = -2;
        } 

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}

