
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Utils.Tasks.Properties;

using TaskScheduler;

namespace NAnt.Utils.Tasks.Windows
{
    [TaskName("at-del-job")]
    public class ATDelJobTask : BaseATTask
    {
        protected override void ExecuteATTask(ScheduledTasks scheduledTasks)
        {
            bool deleted = false;
            string[] taskNames = scheduledTasks.GetTaskNames();
            foreach (string existingTaskName in taskNames)
            {
                if (TaskName.Equals(existingTaskName))
                {
                    Log(Level.Info, Resources.ATDelJobDeleting, TaskName);
                    scheduledTasks.DeleteTask(TaskName);
                    deleted = true;
                }
            }

            if(!deleted)
            {
                Log(Level.Info, Resources.ATDelJobNotFound, TaskName);
            }
        }
    }
}
