using System.Reflection;
using WillCDev.Components;
using WillCDev.Services.Command;

namespace WillCDev.Services
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterServicesFromAssembly(this IServiceCollection services)
        {
            var registry = new CommandRegistry();
            var assembly = typeof(App).Assembly;
            foreach (var type in assembly.GetTypes().Where(t => t.GetCustomAttribute(typeof(ServiceAttribute)) != null))
            {
                var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();
                if (serviceAttribute == null)
                    continue;

                if (serviceAttribute.ServiceType.Equals(ServiceType.Scoped))
                    services.AddScoped(serviceAttribute.InterfaceType, type);
                else if (serviceAttribute.ServiceType.Equals(ServiceType.Singleton))
                    services.AddSingleton(serviceAttribute.InterfaceType, type);
            }
        }
    }
}
