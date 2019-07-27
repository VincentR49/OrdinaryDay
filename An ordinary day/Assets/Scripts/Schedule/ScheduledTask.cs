using System;

[Serializable]
public class ScheduledTask
{
    public DayTime StartTime;
    public DayTime EndTime;
	public int Priority;
    public TaskState State;
    public Task Task;

    public ScheduledTask(ScheduledTask other)
    {
        StartTime = other.StartTime;
        EndTime = other.EndTime;
        Priority = other.Priority;
        State = other.State;
        Task = other.Task;
    }


    public void Reset()
    {
        State = TaskState.ToDo;
    }
}
