using Microsoft.AspNetCore.Components;
using System.Reflection;
using WillCDev.Components;
using WillCDev.Components.Programs;

namespace WillCDev.Services.Program
{
    public static class ProgramRegistrationExtensions
    {
        public static void RegisterProgramsFromAssembly(this IServiceCollection services)
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

            services.AddSingleton<IProgramRegistry>(registry);
        }
    }
}
