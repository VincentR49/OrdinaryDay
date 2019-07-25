using UnityEngine;

/// <summary>
/// Base class that contains all the task performing logic
/// </summary>
public class TaskPerformerDelegate
{
    public delegate void OnTaskFinishedHandler();
    public virtual event OnTaskFinishedHandler OnTaskFinished;

    protected Task _currentTask;


    public virtual void Perform(Task task)
    {
        Debug.Log("Perform " + task.Description);
        _currentTask = task;
    }
}
