using WillCDev.Components.Window;

namespace WillCDev.Components.TaskBar
{
    public class TaskBarProgram
    {
        public string IconName { get; set; } = "Icon Name";
        public EProgram Program { get; set; } = EProgram.StartMenu;
        public bool IsActive { get; set; } = false;
    }
}