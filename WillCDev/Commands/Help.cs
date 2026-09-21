using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("help", Description = new string[]
    {
        "- Format:",
        "   help [command?]\n",
        "- Description:",
        "   Lists available commands || Prints command description.",
        "- Parameters/Flags:",
        "   [command] => Name of command",
        "   -a => list all commands and descriptions",
    })]
    public class Help(ICommandRegistry commandRegistry) : CommandHandlerBase
    {
        private readonly ICommandRegistry _commandRegistry = commandRegistry;

        public override Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            try
            {
                // Check if the user requested verbose output
                bool allMode = args.Contains("-a");
                if (args.Length > 0 && !allMode)
                {
                    // Print the description of the specified command
                    var command = _commandRegistry.GetCommand(args[0]);
                    var descLines = command?.Description.Length > 0 ? command.Description : new string[] { "No description available." };
                    feedController.AddLine(command.Command);
                    foreach (var line in descLines)
                    {
                        feedController.AddLine(line);
                    }
                }
                else
                {
                    // Print the list of available commands
                    var commands = _commandRegistry.GetCommands();
                    foreach (var command in commands)
                    {
                        feedController.AddLine(command.Key);
                        // Print the description of the command if allMode is enabled
                        if (allMode)
                        {
                            var descLines = command.Value.Description.Length > 0 ? command.Value.Description : new string[] { "No description available." };
                            foreach(var line in descLines)
                            { 
                                feedController.AddLine(line); 
                            }
                        }
                    }
                }
            }
            catch (KeyNotFoundException ex)
            {
                feedController.AddLine(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}