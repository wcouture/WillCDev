namespace WillCDev.Services.CommandRegistry.Commands
{
    [Command("clear")]
    public class Clear : CommandHandlerBase
    {
        public override Task HandleCommand(List<string> consoleLines, params string[] args)
        {
            consoleLines.Clear();
            return Task.CompletedTask;
        }
    }
}
