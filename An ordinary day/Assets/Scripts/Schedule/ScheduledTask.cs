using System;

[Serializable]
public class ScheduledTask
{
    public DayTime StartTime;
    public DayTime EndTime;
	public int Priority;
    public bool IsDone;
    public Task Task;
}
