using Splat;

namespace SecureElementReader.App.DependencyInjection
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            ServicesBootstrapper.RegisterServices(services, resolver);
        }
    }
}
