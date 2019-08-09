using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Define a schedule for a given day
/// </summary>
[Serializable]
public class DaySchedule
{
    public DayDate Day;
    public List<ScheduledTask> Tasks;

    public DaySchedule()
    {

    }

    public DaySchedule(DaySchedule other)
    {
        Day = other.Day;
        Tasks = new List<ScheduledTask>();
        foreach (var task in other.Tasks)
            Tasks.Add(new ScheduledTask(task));
    }

    public void Sort()
    {
        Tasks.OrderBy((x) => x.StartTime.ToSeconds());
    }

    public ScheduledTask GetFirstTaskToDoOrFinish()
    {
        if (NTasks == 0)
            return null;
        return Tasks.FirstOrDefault((x) => x.State == TaskState.ToDo || x.State == TaskState.Doing);
    }

    public int NTasks => Tasks == null ? 0 : Tasks.Count;
    public DateTime GetDateTime(DayTime dayTime) => Utils.GetDateTime(Day, dayTime);


    public GamePosition GetLastKnownPosition()
    {
        var lastTask = Tasks.FindLast(task => task.State == TaskState.Done &&
            (task.Task is Move || task.Task is SpawnPNJ));
        if (lastTask == null)
            return null;
        switch (lastTask.Task)
        {
            case Move move:
                return new GamePosition(((Move)lastTask.Task).Destination.Value);
            case SpawnPNJ spawn:
                return new GamePosition(((SpawnPNJ)lastTask.Task).SpawnData);
            default:
                return null;
        }
    }
}
