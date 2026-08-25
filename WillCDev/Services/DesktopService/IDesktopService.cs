using WillCDev.Components.Desktop;

namespace WillCDev.Services.DesktopService
{
    public interface IDesktopService
    {
        public void SubscribeShortcutUpdates(Func<List<DesktopShortcut>, Task> callback);
        public Task AddDesktopShortcut(DesktopShortcut shortcut);
        public Task RemoveDesktopShortcut(int appId);
        public Task NotifySubscribers();
    }
}
