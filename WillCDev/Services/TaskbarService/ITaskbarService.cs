using WillCDev.Components.TaskBar;
using WillCDev.Components.Window;

namespace WillCDev.Services.TaskbarService
{
    public interface ITaskbarService
    {
        public Task AddTaskBarProgram(TaskBarProgram taskBarProgram);
        public Task RemoveTaskBarProgram(EProgram program);
        public Task ToggleTaskBarProgramActiveState(EProgram program, bool isActive);
        public void Subscribe(Func<List<TaskBarProgram>, Task> callback);
    }
}