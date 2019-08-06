using UnityEngine;

/// <summary>
/// Behaviour that manage to perform a task
/// </summary>
public abstract class BasicTaskPerformer : MonoBehaviour
{ 
    // Event related
    public delegate void OnTaskFinishedHandler();
    public event OnTaskFinishedHandler OnTaskFinishedEvent;

    public delegate void OnTaskFailedHandler(int code, string failMessage = "");
    public event OnTaskFailedHandler OnTaskFailedEvent;

    protected Task _task;
    protected float _maxDurationTask;
    protected float _currentTaskDurationSec;
    protected float _initProgressPrc;
    public float ProgressPrc { protected set; get; }
    public bool IsDoingTask => _task != null;

    // Abstract methods
    protected abstract void OnCurrentTaskDurationReachedLimit();
    protected abstract void RefreshCurrentTaskProgress();

    /// <summary>
    /// Perform a given task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="maxDurationSec"></param>
    public virtual void Perform(Task task, float maxDurationSec, float initProgressPrc)
    {
        if (IsDoingTask)
        {
            Debug.LogError("Already perfoming a task: " + _task + ". Canceled current task.");
            Cancel();
        }
        Debug.Log("Perform " + task + ". Max duration: " + maxDurationSec);
        _task = task;
        _initProgressPrc = initProgressPrc;
        ProgressPrc = initProgressPrc;
        _maxDurationTask = maxDurationSec;
        _currentTaskDurationSec = _maxDurationTask * initProgressPrc;
    }


    protected void Update()
    {
        if (IsDoingTask)
        {
            _currentTaskDurationSec += Time.deltaTime;
            RefreshCurrentTaskProgress();
            if (_currentTaskDurationSec >= _maxDurationTask)
            {
                OnCurrentTaskDurationReachedLimit();
            }
        }
    }


    public virtual void Cancel()
    {
        Debug.Log("Cancel task: " + _task);
        Clean();
    }

    /// <summary>
    /// Clean the current task reference and related listeners
    /// Called when a task is ended (canceled, finished, failed)
    /// </summary>
    protected virtual void Clean()
    {
        _task = null;
    }

    /// <summary>
    /// Call when the current task is finished
    /// </summary>
    protected void OnTaskFinished()
    {
        Debug.Log("[TaskPerformer] OnTaskFinished: " + _task);
        OnTaskFinishedEvent?.Invoke();
       
        Clean();
    }

    /// <summary>
    /// Call when the current task is failed
    /// </summary>
    /// <param name="code"></param>
    /// <param name="failMessage"></param>
    protected void OnTaskFailed(int code, string failMessage = "")
    {
        Debug.LogError("[TaskPerformer] OnTaskFailed (" + _task +  "): " + code + " " + failMessage);
        OnTaskFailedEvent?.Invoke(code, failMessage);
        Cancel(); // cancel a current task that have failed
    }
}
