using System.Xml.Serialization;
using WillCDev.Components.TaskBar;
using WillCDev.Components.Window;
using WillCDev.Services.TaskbarService;
namespace WillCDev.Services.WindowService
{
    
    public class WindowService(ITaskbarService taskbarService) : IWindowService
    {
        private readonly ITaskbarService _taskbarService = taskbarService;
        private const int MaximumWindows = 2;
        public ProgramWindow?[] Windows { get => windows; }
        private ProgramWindow?[] windows {get; set;} = new ProgramWindow?[MaximumWindows + 1];
        private int windowCount = 0;
        private List<Func<ProgramWindow?[], Task>> subscribers = new List<Func<ProgramWindow?[], Task>>();
        private List<Func<int, Task>> bringToFrontSubscribers = new List<Func<int, Task>>();

        public async Task OpenWindow(ProgramWindow window)
        {
            if (await CheckExistingWindow(window))
            {
                return;
            }

            if (windowCount < MaximumWindows)
            {
                var emptyIndex = Array.FindIndex(windows, w => w is null);
                if (emptyIndex >= 0)
                {
                    await _taskbarService.AddTaskBarProgram(new TaskBarProgram { IconName = window.WindowTitle, Program = window.Program, IsActive = true });
                    windows[emptyIndex] = window;
                    window.ID = emptyIndex;
                    window.WindowTitle = ProgramWindow.GetProgramTitle(window.Program);
                    windowCount++;
                    await NotifySubscribers();
                } else
                {
                    await WindowLimitReached();
                }
            } else
            {
                await WindowLimitReached();
            }
        }
        public async Task CloseWindow(int windowId)
        {
            var window = windows.FirstOrDefault(w => w?.ID == windowId);
            if (window is not null)
            {
                var index = Array.IndexOf(windows, window);
                if (index >= 0)
                {
                    await _taskbarService.ToggleTaskBarProgramActiveState(window.Program, false);
                    windows[index] = null;
                    windowCount--;
                    if (index == MaximumWindows)
                    {
                        windowCount++;
                    }
                }
            }
            await NotifySubscribers();
        }

        private async Task NotifySubscribers()
        {
            foreach (var subscriber in subscribers)
            {
                await subscriber(windows);
            }
        }

        private async Task NotifyBringToFrontSubscribers(int windowId)
        {
            foreach (var subscriber in bringToFrontSubscribers)
            {
                await subscriber(windowId);
            }
        }

        public int GetMaxWindows()
        {
            return MaximumWindows;
        }

        public int GetWindowCount()
        {
            return windowCount;
        }

        private async Task<bool> CheckExistingWindow(ProgramWindow window)
        {
            var existingWindow = windows.FirstOrDefault(w => w?.Program == window.Program);
            if (existingWindow is not null)
            {
                await NotifyBringToFrontSubscribers(existingWindow.ID);
                return true;
            }
            return false;
        }
        private async Task WindowLimitReached()
        {
            windows[MaximumWindows] = new ProgramWindow
            {
                ID = MaximumWindows,
                WindowTitle = ProgramWindow.GetProgramTitle(EProgram.WindowLimitReached),
                Program = EProgram.WindowLimitReached
            };
            await NotifySubscribers();
        }

        public void Subscribe(Func<ProgramWindow?[], Task> callback)
        {
            subscribers.Add(callback);
        }

        public void SubscribeBringToFront(Func<int, Task> callback)
        {
            bringToFrontSubscribers.Add(callback);
        }
    }
}