using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("echo")]
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
