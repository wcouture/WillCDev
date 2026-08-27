using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("echo")]
    public class Echo : CommandHandlerBase
    {
        public override Task HandleCommand(List<string> consoleLines, params string[] args)
        {
            int numArgs = args.Length;

            string output = string.Join(" ", args);
            consoleLines.Add(output);

            return Task.CompletedTask;
        }
    }
}
