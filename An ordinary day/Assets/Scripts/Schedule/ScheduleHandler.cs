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
    private ScheduledTask _currentTask;
    private bool _isInitialized;
    private bool IsDoingTask => _currentTask != null;


    public void Init(Schedule schedule)
    {
        if (_isInitialized)
        {
            Debug.Log("Schedule handler already initialized");
            return;
        } 
        _schedule = schedule;
        _nextTaskToDo = GetNextTaskToDo();
        _isInitialized = true;
        InitListeners();
    }


    private void OnDestroy()
    {
        CleanListeners();
    }


    private void Update()
    {
        if (!_isInitialized)
            return;
        if (!_schedule.Day.Equals(_currentTime.Value)) // should be the good day
            return;

        if (IsDoingTask)
        {
            if (IsTaskCannotBeDoneOnTime(_currentTask))
            {
                CancelCurrentTask();
            }
        }
        else if (_nextTaskToDo != null && IsTaskReadyToDo(_nextTaskToDo))
        {
            StartTask(_nextTaskToDo);
        }
    }


    private void StartTask(ScheduledTask task)
    {
        _currentTask = task;
        Debug.Log("Start scheduled task. Current time: " + _currentTime.Value + ". Task time: " + task.StartTime);
        _taskPerformer.Perform(task.Task);
        _currentTask.State = ScheduledTask.TaskState.Doing;
        _nextTaskToDo = GetNextTaskToDo();
    }


    private void CancelCurrentTask()
    {
        _taskPerformer.CancelCurrentTask();
        OnCurrentTaskEnded(ScheduledTask.TaskState.Canceled);
    }

    private void OnCurrentTaskEnded(ScheduledTask.TaskState finalState)
    {
        _currentTask.State = finalState;
        _currentTask = null;
        _nextTaskToDo = GetNextTaskToDo();
    }

    #region Listeners
    private void InitListeners()
    {
        _taskPerformer.OnTaskFinishedEvent += OnCurrentTaskFinished;
        _taskPerformer.OnTaskFailedEvent += OnCurrentTaskFailed;
    }


    private void CleanListeners()
    {
        _taskPerformer.OnTaskFinishedEvent -= OnCurrentTaskFinished;
        _taskPerformer.OnTaskFailedEvent -= OnCurrentTaskFailed;
    }


    private void OnCurrentTaskFinished()
    {
        OnCurrentTaskEnded(ScheduledTask.TaskState.Done);
    }


    private void OnCurrentTaskFailed(string failMessage)
    {
        OnCurrentTaskEnded(ScheduledTask.TaskState.Failed);
    }

    #endregion


    #region Utils

    private ScheduledTask GetNextTaskToDo() => _schedule.GetFirstTaskToDo();

    private bool IsTaskReadyToDo(ScheduledTask task) => _schedule.GetDateTime(task.StartTime) <= _currentTime.Value;
    private bool IsTaskCannotBeDoneOnTime(ScheduledTask task) => _schedule.GetDateTime(task.EndTime) <= _currentTime.Value;
    #endregion
}
