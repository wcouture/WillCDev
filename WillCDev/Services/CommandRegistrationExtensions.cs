using System.Reflection;
using WillCDev.Components;
using Shared.Services;
using Shared.Attributes;

namespace WillCDev.Services
{
    public static class CommandRegistrationExtensions
    {
        public static void RegisterCommandsFromAssembly(this IServiceCollection services)
        {
            var registry = new CommandRegistry();
            var assembly = typeof(App).Assembly;
            foreach (var type in assembly.GetTypes().Where(t => typeof(CommandHandlerBase).IsAssignableFrom(t)))
            {
                var attribute = type.GetCustomAttribute<CommandAttribute>();
                if (attribute != null)
                {
                    attribute.HandlerType = type;
                    registry.Register(attribute);
                }
            }
            services.AddSingleton<ICommandRegistry>(registry);
        }
    }
}
