using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RuntimeVariable/Schedule")]
public class RuntimeSchedule : RuntimeVariableData<DaySchedule>
{
	public delegate void ScheduleResetHandler();
    public event ScheduleResetHandler OnScheduleResetEvent;
    
    public void Init(ScheduleData other)
    {
        Value = new DaySchedule(other.Value);
    }


    public void Reset()
    {
        if (Value.NTasks == 0) return;
        foreach (var task in Value.Tasks)
            task.Reset();
        Value.Sort();
        OnScheduleResetEvent?.Invoke();
    }
}
