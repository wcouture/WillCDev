using WillCDev.Components.TaskBar;
using WillCDev.Components.Window;

namespace WillCDev.Services.TaskbarService
{
    public class TaskbarService : ITaskbarService
    {
        private const int MaximumTaskBarPrograms = 32;
        private List<TaskBarProgram> taskBarPrograms { get; set; } = new List<TaskBarProgram>(MaximumTaskBarPrograms);
        private List<Func<List<TaskBarProgram>, Task>> subscribers = new List<Func<List<TaskBarProgram>, Task>>();

        public async Task NotifySubscribers()
        {
            foreach (var subscriber in subscribers)
            {
                await subscriber(taskBarPrograms);
            }
        }

        public async Task AddTaskBarProgram(TaskBarProgram taskBarProgram)
        {
            var existingProgram = taskBarPrograms.FirstOrDefault(t => t.AppId == taskBarProgram.AppId);
            if (existingProgram != null)
            {
                existingProgram.IsActive = taskBarProgram.IsActive;
                await NotifySubscribers();
                return;
            }

            if (taskBarPrograms.Count < MaximumTaskBarPrograms)
            {
                if (taskBarProgram.StartMenu)
                    taskBarPrograms = taskBarPrograms.Prepend(taskBarProgram).ToList();
                else
                    taskBarPrograms.Add(taskBarProgram);
            }
            else
            {
                throw new InvalidOperationException("Maximum number of taskbar programs reached.");
            }
            await NotifySubscribers();
        }

        public async Task RemoveTaskBarProgram(int appId)
        {
            var taskBarProgram = taskBarPrograms.FirstOrDefault(t => t.AppId == appId);
            if (taskBarProgram != null)
            {
                taskBarPrograms.Remove(taskBarProgram);
            }
            await NotifySubscribers();
        }

        public void SubscribeTaskbarUpdates(Func<List<TaskBarProgram>, Task> callback)
        {
            subscribers.Add(callback);
            if (taskBarPrograms.Count > 0)
            {
                _ = callback(taskBarPrograms);
            }
        }

        public async Task ToggleTaskBarProgramActiveState(int appId, bool isActive)
        {
            var taskBarProgram = taskBarPrograms.FirstOrDefault(t => t.AppId == appId);
            if (taskBarProgram != null)
            {
                taskBarProgram.IsActive = isActive;

            }
            await NotifySubscribers();
        }
    }
}