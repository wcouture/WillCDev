namespace WillCDev.Components.Window
{
    public enum Programs
    {
        StartMenu,
        Settings,
        AboutMe,
        Projects,
        Contact,
    }

    public class OpenWindow
    {
        public int ID { get; set; } = 0;
        public string WindowTitle { get; set; } = "Window Title";
        public Programs Program { get; set; } = Programs.StartMenu;
    }
}
