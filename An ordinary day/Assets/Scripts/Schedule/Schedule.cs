using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptables/Schedule")]
public class Schedule : ScriptableObject
{
    public DayDate Day;
    public List<ScheduledTask> Tasks;
}
