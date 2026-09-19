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
            string listHeader = $"{"ID",-5} | {"Program ID",-10} | {"Active",-6} | {"Name", -20}";
            consoleLines.Add(listHeader);
            consoleLines.Add(new string('-', listHeader.Length));

            foreach (var app in apps) 
            {
                bool active = app.IsActive;
                string activeString = active.ToString();
                string appInfo = $"{app.Id,-5} | {app.ProgramId,-15} | {activeString,-6} | {app.Name}";
                consoleLines.Add(appInfo);
            }
            return Task.CompletedTask;
        }
    }
}