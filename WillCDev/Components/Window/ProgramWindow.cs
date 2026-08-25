namespace WillCDev.Components.Window
{
    public enum EProgram
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
        public EProgram Program { get; set; } = EProgram.StartMenu;

        public static string GetProgramTitle(EProgram program)
        {
            return program switch
            {
                EProgram.StartMenu => "Start Menu",
                EProgram.Settings => "Settings",
                EProgram.AboutMe => "About Me",
                EProgram.Projects => "Projects",
                EProgram.Contact => "Contact",
                EProgram.WindowLimitReached => "Window Limit Reached",
                _ => "Unknown Program"
            };
        }
    }
}
