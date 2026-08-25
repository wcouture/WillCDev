using WillCDev.Components.Window;

namespace WillCDev.Services.WindowService
{
    public interface IWindowService
    {
        int GetMaxWindows();
        int GetWindowCount();
        Task OpenWindow(ProgramWindow window);
        Task CloseWindow(int windowId);
        void Subscribe(Func<ProgramWindow?[], Task> callback);
        void SubscribeBringToFront(Func<int, Task> callback);
    }
    
}