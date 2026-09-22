using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("kill", Description = new string[]
    {
        "- Format:",
        "   kill [AppId]",
        "- Description:",
        "   Terminates the specified application.",
        "- Parameters/Flags:",
        "   [AppId] => The ID of the application to terminate",
    })]
    public class Kill(IApplicationService applicationService) : CommandHandlerBase
    {
        public override async Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            if (args.Length == 0)
            {
                feedController.AddLine("Error: No application ID provided.");
                return;
            }

            string appIdStr = args[0];
            try
            {
                int appId = int.Parse(appIdStr);
                await applicationService.StopApplication(appId);

            }
            catch (FormatException ex)
            {
                feedController.AddLine($"Error: Invalid application ID '{appIdStr}'. Please provide a valid integer.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                feedController.AddLine($"Error: Application ID '{appIdStr}' is out of range. Please provide a valid application ID.");
            }
            catch (KeyNotFoundException ex)
            {
                feedController.AddLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                feedController.AddLine($"Error: An unexpected error occurred while trying to kill the application. Details: {ex.Message}");
            }
        }
    }
}
