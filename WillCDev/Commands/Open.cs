using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("open")]
    public class Open(IApplicationService applicationService) : CommandHandlerBase
    {
        public override async Task HandleCommand(int AppId, List<string> consoleLines, params string[] args)
        {
            if (args.Length == 0)
            {
                consoleLines.Add("Error: No application ID provided.");
            }

            try
            {
                string appIdStr = args[0];
                int appId = int.Parse(appIdStr);
                await applicationService.StartApplication(appId);
            }
            catch (FormatException ex)
            {
                consoleLines.Add($"Error: Invalid application ID '{args[0]}'. Please provide a valid integer.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                consoleLines.Add($"Error: Application ID '{args[0]}' is out of range. Please provide a valid application ID.");
            }
            catch (KeyNotFoundException ex)
            {
                consoleLines.Add($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                consoleLines.Add($"Error: An unexpected error occurred while trying to open the application. Details: {ex.Message}");
            }
        }
    }
}
