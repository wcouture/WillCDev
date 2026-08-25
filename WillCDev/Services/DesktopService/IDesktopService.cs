using WillCDev.Components.Desktop;

namespace WillCDev.Services.DesktopService
{
    public interface IDesktopService
    {
        public void SubscribeToDesktopEvents(Func<List<DesktopShortcut>, Task> callback);
        public Task AddDesktopShortcut(DesktopShortcut shortcut);
        public Task RemoveDesktopShortcut(DesktopShortcut shortcut);
    }
}
