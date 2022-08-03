using Splat;
using System.Threading.Tasks;

namespace SecureElementReader.App.DependencyInjection
{
    public static class BackgroundTasksRunner
    {
        public static void Start(IReadonlyDependencyResolver resolver) =>
            Task.Run(() => RunTasksAsync(resolver));

        private static async Task RunTasksAsync(IReadonlyDependencyResolver resolver)
        {
            await InitializeApplicationsList(resolver);
        }

        private static async Task InitializeApplicationsList(IReadonlyDependencyResolver resolver)
        {
            //var applicationService = resolver.GetRequiredService<IApplicationService>();

            //await applicationService.GetInstalledApplicationsAsync();
        }
    }
}
