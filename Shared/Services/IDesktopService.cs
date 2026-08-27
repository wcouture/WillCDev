using Shared.Models;

namespace Shared.Services
{
    public interface IDesktopService
    {
        public void SubscribeShortcutUpdates(Func<List<DesktopShortcut>, Task> callback);
        public Task AddDesktopShortcut(DesktopShortcut shortcut);
        public Task RemoveDesktopShortcut(int appId);
        public Task NotifySubscribers();
    }
}
