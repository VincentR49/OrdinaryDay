using System;

[Serializable]
public class ScheduledTask
{
    public DayTime StartTime;
    public DayTime MaxDuration;
    public DayTime SimulatedDuration;
    public bool StartJustAfterPreviousTask;
    public TaskState State;
    public Task Task;

    public ScheduledTask(ScheduledTask other)
    {
        StartTime = other.StartTime;
        MaxDuration = other.MaxDuration;
        SimulatedDuration = other.SimulatedDuration;
        StartJustAfterPreviousTask = other.StartJustAfterPreviousTask;
        State = other.State;
        Task = other.Task;
    }


    public void Reset()
    {
        State = TaskState.ToDo;
    }
}
