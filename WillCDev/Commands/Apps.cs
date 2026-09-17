using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("apps")]
    public class Apps(IApplicationService applicationService) : CommandHandlerBase
    {
        public override Task HandleCommand(int AppId, List<string> consoleLines, params string[] args)
        {
            var apps = applicationService.GetApplications();
            string listHeader = $"{"ID",-5} | {"Program ID",-12} | {"Name",-25}";
            consoleLines.Add(listHeader);
            consoleLines.Add(new string('-', listHeader.Length));

            foreach (var app in apps) 
            {
                string appId = string.Format("{0,-5}", app?.Id.ToString() ?? "N/A");
                string programId = string.Format("{0,-12}", app?.ProgramId.ToString() ?? "N/A");
                string name = string.Format("{0,-25}", app?.Name ?? "N/A");
                string appInfo = $"{appId} | {programId} | {name}";
                consoleLines.Add(appInfo);
            }
            return Task.CompletedTask;
        }
    }
}
