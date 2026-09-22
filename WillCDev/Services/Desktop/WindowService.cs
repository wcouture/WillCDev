using Shared.Attributes;
using Shared.Services;
using Shared.Models;
using Microsoft.JSInterop;

namespace WillCDev.Services
{
    [Service(typeof(IWindowService), ServiceType.Scoped)]
    public class WindowService(IProgramRegistry programRegistry, IJSRuntime jsRuntime) : IWindowService
    {
        private readonly IProgramRegistry _programRegistry = programRegistry;
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        private const int MaximumWindows = 32;
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
                    windows[emptyIndex] = window;
                    window.ID = emptyIndex;
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
        public async Task CloseWindow(int appId)
        {
            try
            {
                int waitMilliseconds = await _jsRuntime.InvokeAsync<int>("CloseWindow", appId);
                await Task.Delay(waitMilliseconds);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error closing window:", ex.Message);
            }
            var window = windows.FirstOrDefault(w => w?.AppId == appId);
            if (window is not null)
            {
                var index = Array.IndexOf(windows, window);
                if (index >= 0)
                {
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

        public async Task NotifySubscribers()
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
            var existingWindow = windows.FirstOrDefault(w => w?.ProgramId == window.ProgramId);
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
                WindowTitle = "Window Limit Reached",
                ProgramId = _programRegistry.GetProgramIdByProgramName("Window Limit Reached")
            };
            await NotifySubscribers();
        }

        public void SubscribeWindowUpdates(Func<ProgramWindow?[], Task> callback)
        {
            subscribers.Add(callback);
        }

        public void SubscribeBringToFront(Func<int, Task> callback)
        {
            bringToFrontSubscribers.Add(callback);
        }
    }
}