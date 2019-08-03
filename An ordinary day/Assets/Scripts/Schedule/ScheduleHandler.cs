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
    private ScheduledTask _currentTask;
    private bool IsDoingTask => _currentTask != null;
    private bool IsSimulated => _taskPerformer is SimulatedTaskPerformer;

    [Header("Debug")]
    [SerializeField]
    private Schedule schedule;
    [SerializeField]
    private bool _initOnStart;


    private void Start()
    {
        InitTaskPerformerListeners();
        if (_initOnStart)
            Init(schedule);
    }


    public void Init(Schedule schedule)
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
        {
            _currentTask.CompletionPrc = _taskPerformer.ProgressPrc;
            return;
        }
        var taskToDo = GetNextTaskToDo();
        if (taskToDo != null && IsTaskReadyToDo(taskToDo))
        {
            StartTask(taskToDo);
        }
    }


    private void StartTask(ScheduledTask task)
    {
        _currentTask = task;
        Debug.Log(name + " -> Start scheduled task : " + _currentTask + " at Current time: " + _currentTime.Value);
        _currentTask.State = TaskState.Doing;
        _taskPerformer.Perform(task.Task, GetCurrentTaskMaxDuration(), task.CompletionPrc);
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
        if (_schedule != null)
            _schedule.OnScheduleResetEvent -= OnScheduleReset;
    }
    #endregion

    #region Event response

    private void OnCurrentTaskFinished()
    {
        _currentTask.CompletionPrc = 1f;
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
    }

    #endregion


    #region Utils

    private ScheduledTask GetNextTaskToDo() => _schedule.GetFirstTaskToDoOrFinish();
    private bool IsTaskReadyToDo(ScheduledTask task) => _schedule.GetDateTime(task.StartTime) <= _currentTime.Value;

    private float GetCurrentTaskMaxDuration()
            => (IsSimulated ? _currentTask.SimulatedDuration : _currentTask.MaxDuration).ToSeconds()
                    / _inGameSpeedMultiplier.Value;
        
    #endregion
}
