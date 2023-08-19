using MVCToBlazor.Components;

namespace MVCToBlazor
{
    public class TasksRepository
    {
        List<MyTask> myTasks=new List<MyTask>
        {
            new MyTask{Title = "first"},
            new MyTask{Title = "second"},
            new MyTask{Title = "third"},
        };

        public async Task<List<MyTask>> GetMyTasks()
        {
            await Task.Delay(1);
            return myTasks.ToList();
        }

        public async Task<bool> AddTask(MyTask task)
        {
            await Task.Delay(1);
            myTasks.Add(task);
            return true;
        }
    }
}
