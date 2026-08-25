namespace WillCDev.Components.Window
{
    public enum Programs
    {
        StartMenu,
        Settings,
        AboutMe,
        Projects,
        Contact,
        WindowLimitReached
    }



    public class ProgramWindow
    {
        public int ID { get; set; } = 0;
        public string WindowTitle { get; set; } = "Window Title";
        public Programs Program { get; set; } = Programs.StartMenu;

        public static string GetProgramTitle(Programs program)
        {
            return program switch
            {
                Programs.StartMenu => "Start Menu",
                Programs.Settings => "Settings",
                Programs.AboutMe => "About Me",
                Programs.Projects => "Projects",
                Programs.Contact => "Contact",
                Programs.WindowLimitReached => "Window Limit Reached",
                _ => "Unknown Program"
            };
        }
    }
}
