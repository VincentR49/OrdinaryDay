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
    public float CompletionPrc;

    public ScheduledTask(ScheduledTask other)
    {
        StartTime = other.StartTime;
        MaxDuration = other.MaxDuration;
        SimulatedDuration = other.SimulatedDuration;
        StartJustAfterPreviousTask = other.StartJustAfterPreviousTask;
        State = other.State;
        Task = other.Task;
        CompletionPrc = other.CompletionPrc;
    }


    public void Reset()
    {
        State = TaskState.ToDo;
        CompletionPrc = 0f;
    }


    public override string ToString()
    {
        return string.Format("{0} -> {1} - {2} - {3:0.00}", Task.name, StartTime, State, CompletionPrc);
    }
}
