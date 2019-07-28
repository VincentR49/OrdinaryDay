using UnityEngine;

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
        if (_performerHandler == null)
        {
            Debug.LogError("Cannot cancel current task: no performer active");
            return;
        }
        _performerHandler.Cancel();
        base.Cancel();
    }

    #endregion


    protected override void Clean()
    {
        _performerHandler.OnTaskFinishedEvent -= OnTaskFinished;
        _performerHandler.OnTaskFailedEvent -= OnTaskFailed;
        base.Clean();
    }


    private void AddPerformerListeners()
    {
        _performerHandler.OnTaskFinishedEvent += OnTaskFinished;
        _performerHandler.OnTaskFailedEvent += OnTaskFailed;
    }


    protected override void OnCurrentTaskDurationReachedLimit()
    {
        OnTaskFailed(TaskFailedConstants.CouldntFinishOnTime);
    }
}
