using UnityEngine;

/// <summary>
/// Behaviour that handle task
/// </summary>
public class TaskPerformer : MonoBehaviour
{
    // Inspecto
    [SerializeField]
    private TargetReacher _targetReacher;

    // Event related
    public delegate void OnTaskFinishedHandler();
    public virtual event OnTaskFinishedHandler OnTaskFinishedEvent;

    public delegate void OnTaskFailedHandler(string failMessage);
    public virtual event OnTaskFailedHandler OnTaskFailedEvent;

    // Data
    private TaskPerformerDelegate _performer;

    #region Performing Management

    public void Perform(Task task)
    {
        Debug.Log("Perform " + task.Description);
        if (task is Move)
        {
            var movePerformer = new MovePerformer();
            movePerformer.Perform((Move) task, _targetReacher);
            _performer = movePerformer; // cast
        }
        else
        {
            Debug.LogError("Task not implemented yet: " + task);
            return;
        }
        AddPerformerListeners();
    }


    public void CancelCurrentTask()
    {
        if (_performer == null)
        {
            Debug.LogError("Cannot cancel current task: no performer active");
            return;
        }
        _performer.Cancel();
        CleanPerformerListeners();
    }

    #endregion

    #region Events related

    private void OnTaskFinished()
    {
        CleanPerformerListeners();
        Debug.Log("TaskPerformer: OnTaskFinished");
        OnTaskFinishedEvent?.Invoke();
    }


    private void OnTaskFailed(string failMessage)
    {
        CleanPerformerListeners();
        Debug.LogError("TaskPerformer: OnTaskFailed");
        OnTaskFailedEvent?.Invoke(failMessage);
    }


    private void CleanPerformerListeners()
    {
        _performer.OnTaskFinishedEvent -= OnTaskFinished;
        _performer.OnTaskFailedEvent -= OnTaskFailed;
    }


    private void AddPerformerListeners()
    {
        _performer.OnTaskFinishedEvent += OnTaskFinished;
        _performer.OnTaskFailedEvent += OnTaskFailed;
    }
    #endregion
}
