using Microsoft.AspNetCore.Components;
using System.Reflection;
using WillCDev.Components.Desktop;
using WillCDev.Components.Programs;

namespace WillCDev.Services.ProgramService
{
    public static class ProgramRegistrationExtensions
    {
        public static void RegisterProgramsFromAssembly(this IServiceCollection services)
        {
            var registry = new ProgramRegistry();
            var assembly = typeof(Desktop).Assembly;

            foreach (var type in assembly.GetTypes().Where(t => typeof(ComponentBase).IsAssignableFrom(t)))
            {
                var attribute = type.GetCustomAttribute<ProgramAttribute>();
                if (attribute != null)
                {
                    attribute.ComponentType = type;
                    registry.Register(attribute);
                }
            }

            services.AddSingleton<IProgramRegistry>(registry);
        }
    }
}
