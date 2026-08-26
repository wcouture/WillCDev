using Microsoft.AspNetCore.Components;
using WillCDev.Services.Application;

namespace WillCDev.Services.Command.Commands
{
    [Command("echo")]
    public class Echo(IApplicationService applicationService) : CommandHandlerBase
    {
        private readonly IApplicationService _applicationService = applicationService;

        public override Task HandleCommand(List<string> consoleLines, params string[] args)
        {
            int numArgs = args.Length;

            string output = string.Join(" ", args);
            consoleLines.Add(output);

            _applicationService.GetApplications().ForEach(app => { if (app != null) consoleLines.Add(app.Name); });

            return Task.CompletedTask;
        }
    }
}
