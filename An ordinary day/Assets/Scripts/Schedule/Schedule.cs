﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(menuName ="Scriptables/Schedule")]
public class Schedule : ScriptableObject
{
    public DayDate Day;
    public List<ScheduledTask> Tasks;
    public int NTasks => Tasks == null ? 0 : Tasks.Count;

    public void Copy(Schedule other)
    {
        Day = other.Day;
        Tasks = new List<ScheduledTask>();
        foreach (var task in other.Tasks)
            Tasks.Add(new ScheduledTask(task));
    }


    public void Reset()
    {
        if (Tasks == null) return;
        foreach (var task in Tasks)
            task.Reset();
        // We order by start time
        Tasks.OrderBy((x) => x.StartTime.ToSeconds());
    }


    public ScheduledTask GetFirstTaskToDo()
    {
        if (NTasks == 0)
            return null;
        return Tasks.FirstOrDefault((x) => x.State == ScheduledTask.TaskState.ToDo);
    }


    public DateTime GetDateTime(DayTime dayTime) => Utils.GetDateTime(Day, dayTime);
}
