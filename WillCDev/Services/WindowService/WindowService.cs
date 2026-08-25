using System.Xml.Serialization;
using WillCDev.Components.Window;

public class WindowService : IWindowService
{
    private const int MaximumWindows = 2;
    public ProgramWindow?[] Windows { get => windows; }
    private ProgramWindow?[] windows {get; set;} = new ProgramWindow?[MaximumWindows + 1];
    private int windowCount = 0;
    private List<Func<ProgramWindow?[], Task>> subscribers = new List<Func<ProgramWindow?[], Task>>();

    public async Task CloseWindow(int windowId)
    {
        var window = windows.FirstOrDefault(w => w?.ID == windowId);
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

    private async Task NotifySubscribers()
    {
        foreach (var subscriber in subscribers)
        {
            await subscriber(windows);
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

    public async Task OpenWindow(ProgramWindow window)
    {
        if (windowCount < MaximumWindows)
        {
            var emptyIndex = Array.FindIndex(windows, w => w is null);
            if (emptyIndex >= 0)
            {
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

    private async Task WindowLimitReached()
    {
        windows[MaximumWindows] = new ProgramWindow
        {
            ID = MaximumWindows,
            WindowTitle = ProgramWindow.GetProgramTitle(Programs.WindowLimitReached),
            Program = Programs.WindowLimitReached
        };
        await NotifySubscribers();
    }

    public void Subscribe(Func<ProgramWindow?[], Task> callback)
    {
        subscribers.Add(callback);
    }
}