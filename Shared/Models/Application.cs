namespace Shared.Models
{
    public class Application
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProgramId { get; set; } = -1;
    }
}
