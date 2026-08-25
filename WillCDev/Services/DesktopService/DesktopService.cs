using WillCDev.Components.Desktop;

namespace WillCDev.Services.DesktopService
{
    public class DesktopService : IDesktopService
    {
        private const int MaxShortcuts = 10;

        private List<DesktopShortcut> _shortcuts = new();
        private List<Func<List<DesktopShortcut>, Task>> _subscribers = new();

        public async Task AddDesktopShortcut(DesktopShortcut shortcut)
        {
            var existingShortcut = _shortcuts.FirstOrDefault(s => s.ID == shortcut.ID || s.Program == shortcut.Program);
            if (_shortcuts.Count < MaxShortcuts && existingShortcut == null)
            {
                _shortcuts.Add(shortcut);
            }
            await NotifySubscribers();
        }

        public async Task RemoveDesktopShortcut(DesktopShortcut shortcut)
        {
            _shortcuts.Remove(shortcut);
            await NotifySubscribers();
        }

        public void SubscribeToDesktopEvents(Func<List<DesktopShortcut>, Task> callback)
        {
            _subscribers.Add(callback); 
        }

        private async Task NotifySubscribers()
        {
            foreach (var subscriber in _subscribers)
            {
                await subscriber.Invoke(_shortcuts);
            }
        }
    }
}
