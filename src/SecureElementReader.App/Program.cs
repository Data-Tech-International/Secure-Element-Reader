using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using SecureElementReader.App.DependencyInjection;
using Splat;
using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SecureElementReader.App
{
    internal static class Program
    {
        private const int TimeoutSeconds = 3;

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) 
        {
            var mutex = new Mutex(false, typeof(Program).FullName);

            try
            {
                if (!mutex.WaitOne(TimeSpan.FromSeconds(TimeoutSeconds), true))
                {
                    return;
                }

                SubscribeToDomainUnhandledEvents();
                RegisterDependencies();
                //RunBackgroundTasks();
                LogStart();


                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
            }            
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private static void LogStart()
        {
            var logger = Locator.Current.GetRequiredService<ILogger>();
            logger.LogInformation("Application start");            
        }

        private static void SubscribeToDomainUnhandledEvents() =>
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var logger = Locator.Current.GetRequiredService<ILogger>();
            var ex = (Exception)args.ExceptionObject;

            logger.LogCritical($"Unhandled application error: {ex}");
        };

        private static void RegisterDependencies() =>
            Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
