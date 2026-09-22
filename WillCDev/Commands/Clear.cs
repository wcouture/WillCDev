using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("clear", Description = new string[]
    {
        "- Format:",
        "   clear",
        "- Description:",
        "   Clears the feed."
    })]
    public class Clear : CommandHandlerBase
    {
        public override Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            feedController.Clear();
            return Task.CompletedTask;
        }
    }
}
