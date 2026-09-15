namespace Shared.Services
{
    public interface ICommandHandler
    {
        Task HandleCommand(int AppId, List<string> consoleLines, params string[] args);
    }

    public abstract class CommandHandlerBase : ICommandHandler
    {
        public abstract Task HandleCommand(int AppId, List<string> consoleLines, params string[] args);
    }
}
