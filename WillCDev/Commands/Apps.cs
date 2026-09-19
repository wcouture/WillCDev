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
            string listHeader = $"{"ID"}.......... | Program ID | {"Active",-6} | Name";
            consoleLines.Add(listHeader);
            consoleLines.Add(new string('-', listHeader.Length));

            foreach (var app in apps) 
            {
                bool active = app.IsActive;
                string activeString = active.ToString() + (active ? "...." :"...");
                string appInfo = $"{app.Id:D5} | ......{app.ProgramId:D5}...... | {activeString} | {app.Name}";
                consoleLines.Add(appInfo);
            }
            return Task.CompletedTask;
        }
    }
}