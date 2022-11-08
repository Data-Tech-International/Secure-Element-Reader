using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SecureElementReader.App.Views.Titlebars
{
    public partial class MacosTitleBar : UserControl
    {
        private Button closeButton;
        private Button minimizeButton;

        private DockPanel titleBarBackground;
        private StackPanel titleAndWindowIconWrapper;

        public static readonly StyledProperty<bool> IsSeamlessProperty =
        AvaloniaProperty.Register<MacosTitleBar, bool>(nameof(IsSeamless));

        public bool IsSeamless
        {
            get { return GetValue(IsSeamlessProperty); }
            set
            {
                SetValue(IsSeamlessProperty, value);
                if (titleBarBackground != null && titleAndWindowIconWrapper != null)
                {
                    titleBarBackground.IsVisible = IsSeamless;
                    titleAndWindowIconWrapper.IsVisible = IsSeamless;
                }
            }
        }

        public MacosTitleBar()
        {
            this.InitializeComponent();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == false)
            {
                this.IsVisible = false;
            }
            else
            {
                minimizeButton = this.FindControl<Button>("MinimizeButton");
                closeButton = this.FindControl<Button>("CloseButton");
                titleBarBackground = this.FindControl<DockPanel>("TitleBarBackground");
                titleAndWindowIconWrapper = this.FindControl<StackPanel>("TitleAndWindowIconWrapper");
                minimizeButton.Click += MinimizeWindow;
                closeButton.Click += CloseWindow;
                _ = SubscribeToWindowState();
            }
        }

        private void CloseWindow(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window hostWindow = (Window)this.VisualRoot;
            hostWindow.Close();
        }

        private void MinimizeWindow(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window hostWindow = (Window)this.VisualRoot;
            hostWindow.WindowState = WindowState.Minimized;
        }
        private async Task SubscribeToWindowState()
        {
            Window hostWindow = (Window)this.VisualRoot;

            while (hostWindow == null)
            {
                hostWindow = (Window)this.VisualRoot;
                await Task.Delay(50);
            }

            hostWindow.ExtendClientAreaTitleBarHeightHint = 44;
            hostWindow.GetObservable(Window.WindowStateProperty).Subscribe(s =>
            {
                if (s != WindowState.Maximized)
                {
                    hostWindow.Padding = new Thickness(0, 0, 0, 0);
                }
                if (s == WindowState.Maximized)
                {
                    hostWindow.Padding = new Thickness(7, 7, 7, 7);
                }
            });
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
