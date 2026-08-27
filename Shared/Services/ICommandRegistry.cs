using Shared.Attributes;

namespace Shared.Services
{
    public interface ICommandRegistry
    {
        void Register(CommandAttribute command);
        CommandAttribute? GetCommand(string command);
        IDictionary<string, CommandAttribute> GetCommands();
    }
}
