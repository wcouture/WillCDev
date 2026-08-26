namespace WillCDev.Services.CommandRegistry
{
    public interface ICommandHandler
    {
        Task HandleCommand(List<string> consoleLines, params string[] args);
    }

    public abstract class CommandHandlerBase : ICommandHandler
    {
        public abstract Task HandleCommand(List<string> consoleLines, params string[] args);
    }
}
