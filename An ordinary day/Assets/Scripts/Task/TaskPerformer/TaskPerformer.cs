using UnityEngine;
using System.Collections;

/// <summary>
/// Behaviour that handle task for a given game object
/// Delegate the work to TaskPerformerHandler that handles a specific task type.
/// </summary>
public class TaskPerformer : BasicTaskPerformer
{
    [SerializeField]
    private TargetReacher _targetReacher;
    [SerializeField]
    private SpawnPointList _spawnList;

    private TaskPerformerHandler _performerHandler;

    #region Performing Management

    public override void Perform(Task task, float maxDurationSec)
    {
        base.Perform(task, maxDurationSec);
        switch (task)
        {
            case Move move:
                var movePerformer = new MovePerformer();
                _performerHandler = movePerformer; // cast
                AddPerformerListeners();
                movePerformer.Perform(move, _targetReacher);
                break;
            case Spawn spawn:
                var spawnPerformer = new SpawnPerformer();
                _performerHandler = spawnPerformer; // cast
                AddPerformerListeners();
                spawnPerformer.Perform(spawn, _spawnList, gameObject);
                break;
            default:
                Debug.LogError("Task not implemented yet: " + task);
                break;
        }
    }


    public override void Cancel()
    {
        base.Cancel();
        if (_performerHandler == null)
        {
            Debug.LogError("Cannot cancel current task: no performer active");
            return;
        }
        _performerHandler.Cancel();
        CleanPerformerListeners();
    }

    #endregion

    #region Events related

    protected override void OnTaskFinished()
    {
        CleanPerformerListeners();
        base.OnTaskFinished();
    }


    protected override void OnTaskFailed(int code, string failMessage = "")
    {
        Cancel();
        base.OnTaskFailed(code, failMessage);
    }


    private void CleanPerformerListeners()
    {
        _performerHandler.OnTaskFinishedEvent -= OnTaskFinished;
        _performerHandler.OnTaskFailedEvent -= OnTaskFailed;
    }


    private void AddPerformerListeners()
    {
        _performerHandler.OnTaskFinishedEvent += OnTaskFinished;
        _performerHandler.OnTaskFailedEvent += OnTaskFailed;
    }
    #endregion
}
