using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("exit")]
    public class Exit(IApplicationService applicationService) : CommandHandlerBase
    {
        public override Task HandleCommand(int AppId, List<string> consoleLines, params string[] args)
        {
            applicationService.StopApplication(AppId);
            return Task.CompletedTask;
        }
    }
}
