using WillCDev.Services.Command.Commands;

namespace WillCDev.Services.Command
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        private static int _nextId = 1;
        public int Id { get; set; } = -1;
        public string Command { get; set; } = string.Empty;
        public Type HandlerType { get; set; } = typeof(CommandHandlerBase);

        public CommandAttribute(string command)
        {
            Id = _nextId++;
            Command = command.ToUpper();
        }
    }
}
