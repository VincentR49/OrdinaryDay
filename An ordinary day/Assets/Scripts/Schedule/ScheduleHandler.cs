using UnityEngine;

/// <summary>
/// Manage a given schedule.
/// </summary>
public class ScheduleHandler : MonoBehaviour
{
    [SerializeField]
    private BasicTaskPerformer _taskPerformer;
    [SerializeField]
    private DateTimeData _currentTime;
    [SerializeField]
    private FloatData _inGameSpeedMultiplier;

    private Schedule _schedule;
    private ScheduledTask _nextTaskToDo;
    private ScheduledTask _currentTask;
    private bool IsDoingTask => _currentTask != null;
    private bool IsSimulated => _taskPerformer is SimulatedTaskPerformer;


    private void Start()
    {
        InitTaskPerformerListeners();
    }


    public void SetSchedule(Schedule schedule)
    {
        Debug.Log("[ScheduleHandler] SetSchedule of instanciate PNJ");
        if (_schedule != null)
            CleanScheduleListeners();
        _schedule = schedule;
        InitScheduleListeners();
        if (IsDoingTask)
        {
            _taskPerformer.Cancel();
            _currentTask = null;
        }
        _nextTaskToDo = GetNextTaskToDo();
    }


    private void OnDestroy()
    {
        CleanTaskPerformerListeners();
        CleanScheduleListeners();
    }


    private void Update()
    {
        if (_schedule == null)
            return;
        if (!_schedule.Day.Equals(_currentTime.Value)) // should be the good day
            return;
        if (IsDoingTask) // do nothing if already busy
            return;
        if (_nextTaskToDo != null && IsTaskReadyToDo(_nextTaskToDo))
        {
            StartTask(_nextTaskToDo);
        }
    }


    private void StartTask(ScheduledTask task)
    {
        _currentTask = task;
        Debug.Log("Start scheduled task : " + _currentTask.Task + ". Current time: " + _currentTime.Value + ". Task time: " + _currentTask.StartTime);
        _currentTask.State = TaskState.Doing;
        _nextTaskToDo = GetNextTaskToDo();
        _taskPerformer.Perform(task.Task, GetCurrentTaskMaxDuration());
    }


    private void CancelCurrentTask()
    {
        _taskPerformer.Cancel();
        EndCurrentTask(TaskState.Canceled);
    }


    private void EndCurrentTask(TaskState finalState)
    {
        _currentTask.State = finalState;
        _currentTask = null;
        _nextTaskToDo = GetNextTaskToDo();
    }

    #region Listeners
    private void InitTaskPerformerListeners()
    {
        _taskPerformer.OnTaskFinishedEvent += OnCurrentTaskFinished;
        _taskPerformer.OnTaskFailedEvent += OnCurrentTaskFailed;
    }


    private void CleanTaskPerformerListeners()
    {
        _taskPerformer.OnTaskFinishedEvent -= OnCurrentTaskFinished;
        _taskPerformer.OnTaskFailedEvent -= OnCurrentTaskFailed;
    }


    private void InitScheduleListeners()
    {
        _schedule.OnScheduleResetEvent += OnScheduleReset;
    }


    private void CleanScheduleListeners()
    {
        _schedule.OnScheduleResetEvent -= OnScheduleReset;
    }
    #endregion

    #region Event response

    private void OnCurrentTaskFinished()
    {
        EndCurrentTask(TaskState.Done);
    }


    private void OnCurrentTaskFailed(int code, string failMessage)
    {
        EndCurrentTask(TaskState.Failed);
    }


    private void OnScheduleReset()
    {
        if (IsDoingTask)
        {
            _taskPerformer.Cancel();
            _currentTask = null;
        }
        _nextTaskToDo = GetNextTaskToDo();
    }

    #endregion


    #region Utils

    private ScheduledTask GetNextTaskToDo() => _schedule.GetFirstTaskToDo();
    private bool IsTaskReadyToDo(ScheduledTask task) => _schedule.GetDateTime(task.StartTime) <= _currentTime.Value;

    private float GetCurrentTaskMaxDuration()
            => (IsSimulated ? _currentTask.SimulatedDuration : _currentTask.MaxDuration).ToSeconds()
                    / _inGameSpeedMultiplier.Value;
        
    #endregion
}
