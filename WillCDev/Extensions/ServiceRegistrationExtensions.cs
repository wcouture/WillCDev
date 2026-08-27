using System.Reflection;
using WillCDev.Components;
using Shared.Attributes;

namespace WillCDev.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterServicesFromAssembly(this IServiceCollection services, IConfiguration configuration)
        {
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

            // Custom Services
            var servicePath = configuration["DllPluginPath"];
            if (string.IsNullOrEmpty(servicePath))
            {
                throw new DirectoryNotFoundException($"The service path '{servicePath}' is invalid.");
            }

            if (!Directory.Exists(servicePath))
            {
                Directory.CreateDirectory(servicePath);
            }

            var files = Directory.GetFiles(servicePath, "*.dll");
            foreach (var file in files)
            {
                var serviceAssembly = Assembly.LoadFrom(file);
                foreach (var type in serviceAssembly.GetTypes().Where(t => t.GetCustomAttribute(typeof(ServiceAttribute)) != null))
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
}
