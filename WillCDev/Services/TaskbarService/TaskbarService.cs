using WillCDev.Components.TaskBar;
using WillCDev.Components.Window;

namespace WillCDev.Services.TaskbarService
{
    public class TaskbarService : ITaskbarService
    {
        private const int MaximumTaskBarPrograms = 16;
        private List<TaskBarProgram> taskBarPrograms { get; set; } = new List<TaskBarProgram>(MaximumTaskBarPrograms);
        private List<Func<List<TaskBarProgram>, Task>> subscribers = new List<Func<List<TaskBarProgram>, Task>>();

        public TaskbarService()
        {
            taskBarPrograms = new List<TaskBarProgram>(MaximumTaskBarPrograms)
            {
                new TaskBarProgram { IconName = "Fax Sender Information", Program = EProgram.AboutMe, IsActive = false },
                new TaskBarProgram { IconName = "Entire Network", Program = EProgram.Projects, IsActive = false },
                new TaskBarProgram { IconName = "Appearance", Program = EProgram.Settings, IsActive = false },
                new TaskBarProgram { IconName = "Command Prompt", Program = EProgram.WindowLimitReached, IsActive = false }
            };

        }

        private async Task NotifySubscribers()
        {
            foreach (var subscriber in subscribers)
            {
                await subscriber(taskBarPrograms);
            }
        }

        public async Task AddTaskBarProgram(TaskBarProgram taskBarProgram)
        {
            var existingProgram = taskBarPrograms.FirstOrDefault(t => t.Program == taskBarProgram.Program);
            if (existingProgram != null)
            {
                existingProgram.IsActive = taskBarProgram.IsActive;
                await NotifySubscribers();
                return;
            }

            if (taskBarPrograms.Count < MaximumTaskBarPrograms)
            {
                taskBarPrograms.Add(taskBarProgram);
            }
            else
            {
                throw new InvalidOperationException("Maximum number of taskbar programs reached.");
            }
            await NotifySubscribers();
        }

        public async Task RemoveTaskBarProgram(EProgram program)
        {
            var taskBarProgram = taskBarPrograms.FirstOrDefault(t => t.Program == program);
            if (taskBarProgram != null)
            {
                taskBarPrograms.Remove(taskBarProgram);
            }
            await NotifySubscribers();
        }

        public void Subscribe(Func<List<TaskBarProgram>, Task> callback)
        {
            subscribers.Add(callback);
            if (taskBarPrograms.Count > 0)
            {
                _ = callback(taskBarPrograms);
            }
        }

        public async Task ToggleTaskBarProgramActiveState(EProgram program, bool isActive)
        {
            var taskBarProgram = taskBarPrograms.FirstOrDefault(t => t.Program == program);
            if (taskBarProgram != null)
            {
                taskBarProgram.IsActive = isActive;

            }
            await NotifySubscribers();
        }
    }
}