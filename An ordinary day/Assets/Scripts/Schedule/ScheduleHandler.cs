using UnityEngine;

/// <summary>
/// Manage a given schedule.
/// </summary>
public class ScheduleHandler : MonoBehaviour
{
    [SerializeField]
    private BasicTaskPerformer _taskPerformer;
    [SerializeField]
    private RuntimeDateTime _currentTime;
    [SerializeField]
    private FloatData _inGameSpeedMultiplier;

    private RuntimeSchedule _schedule;
    private ScheduledTask _currentTask;
    private bool IsDoingTask => _currentTask != null;
    private bool IsSimulated => _taskPerformer is SimulatedTaskPerformer;
    private DaySchedule CurrentSchedule => _schedule == null ? null : _schedule.Value;


    private void Start()
    {
        InitTaskPerformerListeners();
    }


    public void Init(RuntimeSchedule schedule)
    {
        Debug.Log("[ScheduleHandler] SetSchedule of instanciate PNJ");
        if (_schedule != null)
            CleanScheduleListeners();
        _schedule = schedule;
        InitScheduleListeners();
        if (IsDoingTask)
            Stop();
    }


    public void Stop()
    {
        _taskPerformer.Stop();
        _currentTask = null;
    }


    private void OnDestroy()
    {
        CleanTaskPerformerListeners();
        CleanScheduleListeners();
    }


    private void Update()
    {
        if (CurrentSchedule == null)
            return;
        if (!CurrentSchedule.Day.Equals(_currentTime.Value)) // should be the good day
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
        _taskPerformer.Stop();
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
            _taskPerformer.Stop();
            _currentTask = null;
        }
    }

    #endregion


    #region Utils

    private ScheduledTask GetNextTaskToDo() => CurrentSchedule.GetFirstTaskToDoOrFinish();
    private bool IsTaskReadyToDo(ScheduledTask task) => task.StartJustAfterPreviousTask || CurrentSchedule.GetDateTime(task.StartTime) <= _currentTime.Value;

    private float GetCurrentTaskMaxDuration()
            => (IsSimulated ? _currentTask.SimulatedDuration : _currentTask.MaxDuration).ToSeconds()
                    / _inGameSpeedMultiplier.Value;

    public GamePosition GetLastKnownPosition() => _schedule.GetLastKnownPosition();
    public ScheduledTask GetDoingTask() => _schedule.GetDoingTask();
    #endregion
}
