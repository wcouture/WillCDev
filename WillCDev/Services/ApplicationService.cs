using Shared.Models;
using Shared.Services;
using Shared.Attributes;
using WillCDev.Components.Programs.StartMenu;

namespace WillCDev.Services
{
    [Service(typeof(IApplicationService),ServiceType.Scoped)]
    public class ApplicationService(IWindowService windowService, ITaskbarService taskbarService, IDesktopService desktopService, IProgramRegistry programRegistry) : IApplicationService
    {
        private readonly IWindowService _windowService = windowService;
        private readonly ITaskbarService _taskbarService = taskbarService;
        private readonly IDesktopService _desktopService = desktopService;
        private readonly IProgramRegistry _programRegistry = programRegistry;

        private List<Func<List<DesktopShortcut>, Task>> _desktopCallbacks = new();
        private List<Func<List<TaskBarProgram>, Task>> _taskbarCallbacks = new();
        private List<Func<ProgramWindow?[], Task>> _windowCallbacks = new();
        private List<Func<int, Task>> _windowForwardCallbacks = new();

        private const int MaxApplications = 32;
        private int _applicationCount = 0;
        private Application?[] applications = new Application?[MaxApplications];

        public async Task Init()
        {
            _desktopService.SubscribeShortcutUpdates(OnUpdateDesktop);
            _windowService.SubscribeWindowUpdates(OnUpdateWindows);
            _windowService.SubscribeBringToFront(OnBringWindowForward);
            _taskbarService.SubscribeTaskbarUpdates(OnUpdateTaskBar);
            await LoadDefaultPrograms();
        }

        private async Task LoadDefaultPrograms()
        {
            var programs = _programRegistry.GetProgramAttributes();

            foreach (var program in programs)
            {
                ProgramAttribute programAttribute = program.Value;
                if (programAttribute != null)
                {
                    var app = new Application()
                    {
                        Name = programAttribute.Name,
                        Description = programAttribute.Description,
                        IconName = programAttribute.IconName,
                        ProgramId = programAttribute.ID,
                    };
                    var appId = await AddApplication(app);
                    
                    if (programAttribute.DefaultTaskbarIcon)
                        await CreateTaskBarIcon(appId);
                    if (programAttribute.DefaultDesktopShortcut)
                        await CreateDesktopShortcut(appId);
                }
            }
        }

        public List<Application?> GetApplications() => applications.ToList();

        public Task<int> AddApplication(Application application)
        {
            if (_applicationCount < MaxApplications)
            {
                var emptyIndex = Array.FindIndex(applications, app => app == null);
                applications[emptyIndex] = application;
                applications[emptyIndex]!.Id = emptyIndex;
                _applicationCount++;
                return Task.FromResult(emptyIndex);
            }
            return Task.FromResult(-1);
        }
        public Task RemoveApplication(int appId)
        {
            if (appId <= -1) return Task.CompletedTask;

            var app = applications[appId];
            if (app != null && app.Id == appId)
            {
                applications[appId] = null;
                _applicationCount--;
            }
            return Task.CompletedTask;
        }

        public async Task StartApplication(int appId)
        {
            if (appId <= -1) return;

            var application = applications[appId];
            if (application is not null && application.Id == appId)
            {
                ProgramWindow window = new ProgramWindow()
                {
                    AppId = appId,
                    ProgramId = application.ProgramId,
                    WindowTitle = application.Name,
                };
                await _windowService.OpenWindow(window);
                await _taskbarService.ToggleTaskBarProgramActiveState(appId, true);
            }
        }

        public async Task StopApplication(int appId)
        {
            if (appId <= -1) return;

            var application = applications[appId];
            if (application is not null && application.Id == appId)
            {
                await _windowService.CloseWindow(appId);
                await _taskbarService.ToggleTaskBarProgramActiveState(appId, false);
            }
        }

        public async Task CreateDesktopShortcut(int appId)
        {
            if (appId <= -1) return;

            var application = applications[appId];
            if (application is not null && application.Id == appId)
            {
                DesktopShortcut shortcut = new()
                {
                    AppId = appId,
                    IconName = application.IconName,
                    Label = application.Name
                };
                await _desktopService.AddDesktopShortcut(shortcut);
            }
        }

        public async Task CreateTaskBarIcon(int appId)
        {
            if (appId <= -1) return;

            var application = applications[appId];
            if (application is not null && application.Id == appId)
            {
                var programAttr = _programRegistry.GetProgram(application.ProgramId);

                TaskBarProgram program = new()
                {
                    AppId = appId,
                    IconName = application.IconName,
                    IsActive = false,
                    StartMenu = programAttr != null ? programAttr.ComponentType == typeof(StartMenu) : false
                };
                await _taskbarService.AddTaskBarProgram(program);
            }
        }

        public async Task DeleteDesktopShortcut(int appId)
        {
            if (appId <= -1) return;

            var application = applications[appId];
            if (application is not null && application.Id == appId)
            {
                await _desktopService.RemoveDesktopShortcut(appId);
            }
        }

        public async Task DeleteTaskBarIcon(int appId)
        {
            if (appId <= -1) return;

            var application = applications[appId];
            if (application is not null && application.Id == appId)
            {
                await _taskbarService.RemoveTaskBarProgram(appId);  
            }
        }

        public void SubscribeToUpdatesDesktop(Func<List<DesktopShortcut>, Task> callback)
        {
            _desktopCallbacks.Add(callback);
        }

        public void SubscribeToUpdatesWindows(Func<ProgramWindow?[], Task> callback)
        {
            _windowCallbacks.Add(callback);
        }

        public void SubscribeToBringToFrontWindows(Func<int, Task> callback)
        {
            _windowForwardCallbacks.Add(callback);
        }

        public void SubscribeToUpdatesTaskBar(Func<List<TaskBarProgram>, Task> callback)
        {
            _taskbarCallbacks.Add(callback);
        }

        public async Task RefreshApplicationData()
        {
            await _desktopService.NotifySubscribers();
            await _taskbarService.NotifySubscribers();
            await _windowService.NotifySubscribers();
        }

        private async Task OnUpdateDesktop(List<DesktopShortcut> shortcuts)
        {
            foreach (var callback in _desktopCallbacks)
            {
                await callback(shortcuts);
            }
        }

        private async Task OnUpdateWindows(ProgramWindow?[] windows)
        {
            foreach (var callback in _windowCallbacks)
            {
                await callback(windows);
            }
        }

        private async Task OnUpdateTaskBar(List<TaskBarProgram> programs)
        {
            foreach (var callback in _taskbarCallbacks)
            {
                await callback(programs);
            }
        }

        private async Task OnBringWindowForward(int windowId)
        {
            foreach (var callback in _windowForwardCallbacks)
            {
                await callback(windowId);
            }
        }
    }
}
