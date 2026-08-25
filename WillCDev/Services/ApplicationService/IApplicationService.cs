using WillCDev.Components.Desktop;
using WillCDev.Components.TaskBar;
using WillCDev.Components.Window;

namespace WillCDev.Services.ApplicationService
{
    public interface IApplicationService
    {
        public Task Init();

        public List<Application?> GetApplications();

        public Task<int> AddApplication(Application application);
        public Task RemoveApplication(int appId);

        public Task StartApplication(int appId);
        public Task StopApplication(int appId);

        public Task CreateDesktopShortcut(int appId);
        public Task DeleteDesktopShortcut(int appId);

        public Task CreateTaskBarIcon(int appId);
        public Task DeleteTaskBarIcon(int appId);

        public void SubscribeToUpdatesDesktop(Func<List<DesktopShortcut>, Task> callbacks);
        public void SubscribeToUpdatesTaskBar(Func<List<TaskBarProgram>, Task> callbacks);
        public void SubscribeToUpdatesWindows(Func<ProgramWindow?[], Task> callbacks);
        public void SubscribeToBringToFrontWindows(Func<int, Task> callback);

        public Task RefreshApplicationData();
    }
}
