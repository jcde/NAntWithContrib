using NAnt.Core.Attributes;

using TaskScheduler;

using Task = NAnt.Core.Task;

namespace NAnt.Utils.Tasks.Windows
{
    public abstract class BaseATTask : Task
    {
        private string taskName;
        [TaskAttribute("name")]
        public string TaskName
        {
            get
            {
                if (!taskName.EndsWith(".job"))
                {
                    taskName = string.Concat(taskName, ".job");
                }
                return taskName;
            }
            set { taskName = value; }
        }

        protected sealed override void ExecuteTask()
        {
            using (ScheduledTasks scheduledTasks = new ScheduledTasks())
            {
                ExecuteATTask(scheduledTasks);
            }
        }

        protected abstract void ExecuteATTask(ScheduledTasks scheduledTasks);
    }
}
