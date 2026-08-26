using WillCDev.Components.Desktop;

namespace WillCDev.Services.Desktop
{
    [Service(typeof(IDesktopService), ServiceType.Scoped)]
    public class DesktopService : IDesktopService
    {
        private const int MaxShortcuts = 32;

        private List<DesktopShortcut> _shortcuts = new();
        private List<Func<List<DesktopShortcut>, Task>> _subscribers = new();

        public async Task AddDesktopShortcut(DesktopShortcut shortcut)
        {
            var existingShortcut = _shortcuts.FirstOrDefault(s => s.AppId == shortcut.AppId);
            if (_shortcuts.Count < MaxShortcuts && existingShortcut == null)
            {
                _shortcuts.Add(shortcut);
                await NotifySubscribers();
            }
        }

        public async Task RemoveDesktopShortcut(int appId)
        {
            var shortcut = _shortcuts.FirstOrDefault(s => s.AppId == appId);
            if (shortcut != null)
            {
                _shortcuts.Remove(shortcut);
                await NotifySubscribers();
            }
        }

        public void SubscribeShortcutUpdates(Func<List<DesktopShortcut>, Task> callback)
        {
            _subscribers.Add(callback); 
        }

        public async Task NotifySubscribers()
        {
            foreach (var subscriber in _subscribers)
            {
                await subscriber.Invoke(_shortcuts);
            }
        }
    }
}
