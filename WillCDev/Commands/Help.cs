using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("help")]
    public class Help(ICommandRegistry commandRegistry) : CommandHandlerBase
    {
        private readonly ICommandRegistry _commandRegistry = commandRegistry;

        public override Task HandleCommand(List<string> consoleLines, params string[] args)
        {
            var commands = _commandRegistry.GetCommands();

            foreach (var command in commands)
            {
                consoleLines.Add(command.Key);
            }

            return Task.CompletedTask;
        }
    }
}