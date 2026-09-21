using Shared.Attributes;
using Shared.Services;

namespace WillCDev.Commands
{
    [Command("apps")]
    public class Apps(IApplicationService applicationService) : CommandHandlerBase
    {
        public override Task HandleCommand(int AppId, IFeedController feedController, params string[] args)
        {
            var apps = applicationService.GetApplications();
            string listHeader = $"{"ID",-5} | {"Program ID",-10} | {"Active",-6} | {"Name", -20}";
            feedController.AddLine(listHeader);
            feedController.AddLine(new string('-', listHeader.Length));

            foreach (var app in apps) 
            {
                bool active = app.IsActive;
                string activeString = active.ToString();
                string appInfo = $"{app.Id,-5} | {app.ProgramId,-10} | {activeString,-6} | {app.Name}";
                feedController.AddLine(appInfo);
            }
            return Task.CompletedTask;
        }
    }
}