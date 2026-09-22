using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("echo", Description = new string[]
    {
        "- Format:",
        "   echo [text]",
        "- Description:",
        "   Prints the specified text to the feed.",
        "- Parameters/Flags:",
        "   [text] => The text to print",
    })]
    public class Echo : CommandHandlerBase
    {
        public override Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            int numArgs = args.Length;

            string output = string.Join(" ", args);
            feedController.AddLine(output);

            return Task.CompletedTask;
        }
    }
}
