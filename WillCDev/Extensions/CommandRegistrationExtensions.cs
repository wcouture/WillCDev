using System.Reflection;
using WillCDev.Components;
using Shared.Services;
using Shared.Attributes;
using WillCDev.Services;

namespace WillCDev.Extensions
{
    public static class CommandRegistrationExtensions
    {
        public static void RegisterCommandsFromAssembly(this IServiceCollection services, IConfiguration configuration)
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

            // Custom commands
            var commandPath = configuration["DllPluginPath"];
            if (string.IsNullOrEmpty(commandPath))
            {
                throw new DirectoryNotFoundException($"The command path '{commandPath}' is invalid.");
            }

            if (!Directory.Exists(commandPath))
            {
                Directory.CreateDirectory(commandPath);
            }

            var files = Directory.GetFiles(commandPath, "*.dll");
            foreach (var file in files)
            {
                var commandAssembly = Assembly.LoadFrom(file);
                foreach (var type in commandAssembly.GetTypes().Where(t => typeof(CommandHandlerBase).IsAssignableFrom(t)))
                {
                    var attribute = type.GetCustomAttribute<CommandAttribute>();
                    if (attribute != null)
                    {
                        attribute.HandlerType = type;
                        registry.Register(attribute);
                    }
                }
            }

            services.AddSingleton<ICommandRegistry>(registry);
        }
    }
}
