using UnityEngine;

/// <summary>
/// Manage a given schedule.
/// </summary>
public class ScheduleHandler : MonoBehaviour
{
    [SerializeField]
    private TaskPerformer _taskPerformer;
    [SerializeField]
    private DateTimeData _currentTime;

    private Schedule _schedule;
    private ScheduledTask _nextTaskToDo;
    private bool _isInitialized;

    public void Init(Schedule schedule)
    {
        _schedule = schedule;
        _nextTaskToDo = _schedule.GetFirstTaskToDo();
        _isInitialized = true;
    }


    private void Update()
    {
        // TODO manage here the change of status of the different tasks
        if (!_isInitialized)
            return;
        if (_nextTaskToDo != null
                && _nextTaskToDo.State == ScheduledTask.TaskState.ToDo
                && _schedule.Day.Equals(_currentTime.Value))
        {
            var taskDateTime = _schedule.GetDateTime(_nextTaskToDo.StartTime);
            if (taskDateTime <= _currentTime.Value)
            {
                Debug.Log("Start scheduled task. Current time: " + _currentTime.Value + ". Task time: " + taskDateTime);
                _taskPerformer.Perform(_nextTaskToDo.Task);
                _nextTaskToDo.State = ScheduledTask.TaskState.Doing;
            }
        }
    }
}
