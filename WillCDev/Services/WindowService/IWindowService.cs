using WillCDev.Components.Window;

namespace WillCDev.Services.Window
{
    public interface IWindowService
    {
        Task NotifySubscribers();
        int GetMaxWindows();
        int GetWindowCount();
        Task OpenWindow(ProgramWindow window);
        Task CloseWindow(int appId);
        void SubscribeWindowUpdates(Func<ProgramWindow?[], Task> callback);
        void SubscribeBringToFront(Func<int, Task> callback);
    }
    
}