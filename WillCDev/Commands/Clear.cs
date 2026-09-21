using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("clear")]
    public class Clear : CommandHandlerBase
    {
        public override Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            feedController.Clear();
            return Task.CompletedTask;
        }
    }
}
