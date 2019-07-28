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
    public bool IsDoingTask => _task != null;


    protected abstract void OnCurrentTaskDurationReachedLimit();


    /// <summary>
    /// Perform a given task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="maxDurationSec"></param>
    public virtual void Perform(Task task, float maxDurationSec)
    {
        Debug.Log("Perform " + task + ". Max duration: " + maxDurationSec);
        _task = task;
        _maxDurationTask = maxDurationSec;
        _currentTaskDurationSec = 0f;
    }


    protected void Update()
    {
        if (IsDoingTask)
        {
            _currentTaskDurationSec += Time.deltaTime;
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
        _maxDurationTask = 0f;
        _currentTaskDurationSec = 0f;
    }

    /// <summary>
    /// Call when the current task is finished
    /// </summary>
    protected virtual void OnTaskFinished()
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
    protected virtual void OnTaskFailed(int code, string failMessage = "")
    {
        Debug.LogError("[TaskPerformer] OnTaskFailed (" + _task +  "): " + code + " " + failMessage);
        OnTaskFailedEvent?.Invoke(code, failMessage);
        Cancel(); // cancel a current task that have failed
    }
}
