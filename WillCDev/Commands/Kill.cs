using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("kill")]
    public class Kill(IApplicationService applicationService) : CommandHandlerBase
    {
        public override async Task HandleCommand(int AppId, List<string> consoleLines, params string[] args)
        {
            if (args.Length == 0)
            {
                consoleLines.Add("Error: No application ID provided.");
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
                consoleLines.Add($"Error: Invalid application ID '{appIdStr}'. Please provide a valid integer.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                consoleLines.Add($"Error: Application ID '{appIdStr}' is out of range. Please provide a valid application ID.");
            }
            catch (KeyNotFoundException ex)
            {
                consoleLines.Add($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                consoleLines.Add($"Error: An unexpected error occurred while trying to kill the application. Details: {ex.Message}");
            }
        }
    }
}
