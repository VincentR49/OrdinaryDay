using UnityEngine;

/// <summary>
/// Base class for class containing all the task performing logic
/// </summary>
public abstract class TaskPerformerDelegate
{
    // Events
    public delegate void OnTaskFinishedHandler();
    public virtual event OnTaskFinishedHandler OnTaskFinishedEvent;

    public delegate void OnTaskFailedHandler(string failMessage);
    public virtual event OnTaskFailedHandler OnTaskFailedEvent;

    protected Task _currentTask;

    /// <summary>
    /// Base performing method
    /// </summary>
    /// <param name="task"></param>
    protected void Perform(Task task)
    {
        Debug.Log("Perform " + task);
        _currentTask = task;
    }

    /// <summary>
    /// Cancel the current task
    /// </summary>
    public virtual void Cancel()
    {
        Debug.Log("Cancel task: " + _currentTask);
        CleanListeners();
    }

    public abstract void CleanListeners();


    #region Events related methods
    /// <summary>
    /// Call when finishing an event
    /// </summary>
    protected void OnTaskFinished()
    {
        Debug.Log("OnTaskFinished: " + _currentTask);
        CleanListeners();
        OnTaskFinishedEvent?.Invoke();
    }


    protected void OnTaskFailed(string failMessage)
    {
        Debug.LogError("OnTaskFailed: " + _currentTask + ": " + failMessage);
        CleanListeners();
        OnTaskFailedEvent?.Invoke(failMessage);
    }
    #endregion
}
