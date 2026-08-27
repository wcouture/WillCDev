namespace Shared.Models
{
    public class TaskBarProgram
    {
        public string IconName { get; set; } = "Icon Name";
        public int AppId { get; set; } = -1;
        public bool IsActive { get; set; } = false;
        public bool StartMenu { get; set; } = false;
    }
}