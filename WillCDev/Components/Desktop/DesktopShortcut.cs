using WillCDev.Components.Window;

namespace WillCDev.Components.Desktop
{
    public class DesktopShortcut
    {
        public int ID { get; set; } = 0;
        public string Label { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public int AppId { get; set; } = -1;
    }
}
