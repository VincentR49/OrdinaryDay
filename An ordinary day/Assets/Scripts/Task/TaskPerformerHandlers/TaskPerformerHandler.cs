using UnityEngine;

/// <summary>
/// Manage the task performing logic of a specific task
/// Only one task can be handle at the same time by a TaskPerformerHandler
/// </summary>
public abstract class TaskPerformerHandler
{
    public delegate void OnTaskFinishedHandler();
    public virtual event OnTaskFinishedHandler OnTaskFinishedEvent;

    public delegate void OnTaskFailedHandler(int code, string failMessage = "");
    public virtual event OnTaskFailedHandler OnTaskFailedEvent;

    protected Task _currentTask;
    public bool IsDoingTask => _currentTask != null;

    /// <summary>
    /// Base performing method
    /// Warning: To call on the child class
    /// </summary>
    /// <param name="task"></param>
    protected void Perform(Task task)
    {
        if (IsDoingTask)
        {
            Debug.LogError("Cannot perform several tasks simultaneously");
            return;
        }
        Debug.Log("[TaskPeformerHandler] Handle task " + task);
        _currentTask = task;
    }

    /// <summary>
    /// Cancel the current task
    /// </summary>
    public virtual void Cancel()
    {
        Debug.Log("[TaskPeformerHandler] Cancel task: " + _currentTask);
        Clean();
    }

    protected virtual void Clean()
    {
        _currentTask = null;
    }


    #region Events related methods
    /// <summary>
    /// Call when finishing an event
    /// </summary>
    protected void OnTaskFinished()
    {
        Debug.Log("OnTaskFinished: " + _currentTask);     
        OnTaskFinishedEvent?.Invoke();
        Clean();
    }


    protected void OnTaskFailed(int code, string failMessage = "")
    {
        Debug.LogError("OnTaskFailed: " + _currentTask + ": " + code + " " + failMessage);
        OnTaskFailedEvent?.Invoke(code, failMessage);
        Clean();
    }
    #endregion
}
