using Shared.Attributes;
using Shared.Services;
namespace WillCDev.Services
{
    public class CommandRegistry() : ICommandRegistry
    {
        private readonly Dictionary<string, CommandAttribute> _commands = new();

        public void Register(CommandAttribute command)
        {
            if (!_commands.ContainsKey(command.Command))
            {
                // Add the command to the dictionary
                _commands.Add(command.Command, command);
            }
            else
            {
                // Replace the existing command attribute with the new one
                _commands.Remove(command.Command);
                _commands.Add(command.Command, command);
            }
        }
        public CommandAttribute? GetCommand(string command)
        {
            if (_commands.TryGetValue(command.ToLower(), out var commandAttribute))
            {
                return commandAttribute;
            }
            throw new KeyNotFoundException($"Command '{command}' not found.");
        }
        public IDictionary<string, CommandAttribute> GetCommands()
        {
            return _commands;
        }
    }
}
