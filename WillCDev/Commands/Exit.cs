using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("exit")]
    public class Exit(IApplicationService applicationService) : CommandHandlerBase
    {
        public override async Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            try
            {
                await applicationService.StopApplication(AppId);
            }
            catch (Exception ex)
            {
                feedController.AddLine($"Error: An unexpected error occurred while trying to exit the application. Details: {ex.Message}");
            }
        }
    }
}
