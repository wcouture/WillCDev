using Shared.Models;

namespace Shared.Services
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