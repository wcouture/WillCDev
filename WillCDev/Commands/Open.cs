using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("open", Description = new string[]
    {
        "- Format:",
        "   open [AppId]",
        "- Description:",
        "   Opens the specified application.",
        "- Parameters/Flags:",
        "   [AppId] => The ID of the application to open",
    })]
    public class Open(IApplicationService applicationService) : CommandHandlerBase
    {
        public override async Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            if (args.Length == 0)
            {
                feedController.AddLine("Error: No application ID provided.");
                return;
            }

            try
            {
                string appIdStr = args[0];
                int appId = int.Parse(appIdStr);
                await applicationService.StartApplication(appId);
            }
            catch (FormatException ex)
            {
                feedController.AddLine($"Error: Invalid application ID '{args[0]}'. Please provide a valid integer.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                feedController.AddLine($"Error: Application ID '{args[0]}' is out of range. Please provide a valid application ID.");
            }
            catch (KeyNotFoundException ex)
            {
                feedController.AddLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                feedController.AddLine($"Error: An unexpected error occurred while trying to open the application. Details: {ex.Message}");
            }
        }
    }
}
