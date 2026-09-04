using Microsoft.AspNetCore.Components;
using System.Reflection;
using WillCDev.Components;
using Shared.Attributes;
using Shared.Services;
using WillCDev.Services;
using Shared.Models;

namespace WillCDev.Extensions
{
    public static class ProgramRegistrationExtensions
    {
        public static void RegisterProgramsFromAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var registry = new ProgramRegistry();
            var assembly = typeof(App).Assembly;

            foreach (var type in assembly.GetTypes().Where(t => typeof(ComponentBase).IsAssignableFrom(t)))
            {
                var attribute = type.GetCustomAttribute<ProgramAttribute>();
                if (attribute != null)
                {
                    attribute.ComponentType = type;
                    registry.Register(attribute);
                }
            }

            // Custom programs
            var programPath = configuration["DllPluginPath"];
            if (string.IsNullOrEmpty(programPath))
            {
                throw new DirectoryNotFoundException($"The program path '{programPath}' is invalid.");
            }

            if (!Directory.Exists(programPath))
            {
                Directory.CreateDirectory(programPath);
            }

            var files = Directory.GetFiles(programPath, "*.dll");
            foreach (var file in files)
            {
                var programAssembly = Assembly.LoadFrom(file);
                foreach (var type in programAssembly.GetTypes().Where(t => typeof(ComponentBase).IsAssignableFrom(t)))
                {
                    var attribute = type.GetCustomAttribute<ProgramAttribute>();
                    if (attribute != null)
                    {
                        attribute.ComponentType = type;
                        registry.Register(attribute);
                    }
                }
            }

            services.AddSingleton<IProgramRegistry>(registry);
        }
    }
}
