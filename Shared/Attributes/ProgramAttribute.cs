namespace Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProgramAttribute : Attribute
    {
        private static int ProgramCount { get; set; } = 1000;
        public int ID { get; set; } = 0;

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public bool DefaultTaskbarIcon { get; set; } = true;
        public bool DefaultDesktopShortcut { get; set; } = false;
        public Type ComponentType { get; set; } = typeof(object);
        public bool ShrinkWindowOnLoad { get; set; } = false;

        public ProgramAttribute(string programName, 
                                string programDescription = "New Program", 
                                string iconName = "Generic Document", 
                                bool defaultTaskbarIcon = true, 
                                bool defaultDesktopShortcut = false,
                                bool shrinkWindowOnLoad = false)
        {
            ID = ProgramAttribute.ProgramCount + 1;
            ProgramAttribute.ProgramCount++;

            Name = programName;
            Description = programDescription;
            IconName = iconName;
            DefaultDesktopShortcut = defaultDesktopShortcut;
            DefaultTaskbarIcon = defaultTaskbarIcon;
            ShrinkWindowOnLoad = shrinkWindowOnLoad;
        }
    }
}
