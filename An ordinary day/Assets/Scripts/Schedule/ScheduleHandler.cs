using UnityEngine;

/// <summary>
/// Manage a given schedule.
/// </summary>
public class ScheduleHandler : MonoBehaviour
{
    private Schedule _schedule;

    public void Init(Schedule schedule)
    {
        _schedule = schedule;
    }
}
