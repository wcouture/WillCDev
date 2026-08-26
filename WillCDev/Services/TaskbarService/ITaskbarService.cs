using WillCDev.Components.TaskBar;
using WillCDev.Components.Window;

namespace WillCDev.Services.Taskbar
{
    public interface ITaskbarService
    {
        public Task AddTaskBarProgram(TaskBarProgram taskBarProgram);
        public Task RemoveTaskBarProgram(int appId);
        public Task ToggleTaskBarProgramActiveState(int appId, bool isActive);
        public void SubscribeTaskbarUpdates(Func<List<TaskBarProgram>, Task> callback);
        public Task NotifySubscribers();
    }
}