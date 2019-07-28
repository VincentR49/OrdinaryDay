using UnityEngine;
using System.Collections;
/// <summary>
/// Behaviour that manage to perform a task
/// </summary>
public abstract class BasicTaskPerformer : MonoBehaviour
{ 
    // Event related
    public delegate void OnTaskFinishedHandler();
    public virtual event OnTaskFinishedHandler OnTaskFinishedEvent;

    public delegate void OnTaskFailedHandler(int code, string failMessage = "");
    public virtual event OnTaskFailedHandler OnTaskFailedEvent;

    protected Task _task;
    protected float _maxDurationTask;
    protected float _currentTaskDurationSec;

    public bool IsDoingTask => _task != null;


    // make it virtual and implement some stuff
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


    protected virtual void OnCurrentTaskDurationReachedLimit()
    {
        OnTaskFailed(TaskFailedConstants.CouldntFinishOnTime);
    }


    public virtual void Cancel()
    {
        Debug.Log("Cancel task: " + _task);
        EndCurrentTask();
    }


    protected virtual void EndCurrentTask()
    {
        _task = null;
        _maxDurationTask = 0f;
        _currentTaskDurationSec = 0f;
    }


    protected virtual void OnTaskFinished()
    {
        Debug.Log("[TaskPerformer] OnTaskFinished: " + _task);
        OnTaskFinishedEvent?.Invoke();
        EndCurrentTask();
    }


    protected virtual void OnTaskFailed(int code, string failMessage = "")
    {
        Debug.LogError("[TaskPerformer] OnTaskFailed (" + _task +  "): " + code + " " + failMessage);
        OnTaskFailedEvent?.Invoke(code, failMessage);
        EndCurrentTask();
    }
}
