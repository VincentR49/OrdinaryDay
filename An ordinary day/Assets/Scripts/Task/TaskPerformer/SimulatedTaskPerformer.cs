using UnityEngine;

/// <summary>
/// Simulated task Performer.
/// Basic TaskPerformer that ends automatically a task after reaching the defined duration.
/// </summary>
public class SimulatedTaskPerformer : BasicTaskPerformer
{
    [SerializeField]
    protected PositionTracker _positionTracker;

    public override void Perform(Task task, float maxDurationSec, float initProgressPrc)
    {
        base.Perform(task, maxDurationSec, initProgressPrc);
        switch (task)
        {
            case SpawnPNJ spawn:
                PerformSpawn(spawn);
                break;
            case Move move:
                // do nothing
                break;
            default:
                Debug.LogError("Task not implemented yet for simulation: " + task);
                break;
        }
    }

    #region Spawn
    public void PerformSpawn(SpawnPNJ spawn)
    {
        if (spawn.IsInCurrentScene())
        {
            var pnj = PNJInstancier.InstanciatePNJ(spawn.PNJ, spawn.SpawnData);
            if (pnj == null) // couldnt instanciate the pnj in the current scene for some reason
            {
                OnTaskFailed(TaskFailedConstants.SpawnPointNotFound);
                return;
            }
        }
        OnTaskFinished();
    }
    #endregion

    // Finish task when we reach the limit duration
    protected override void OnCurrentTaskDurationReachedLimit()
    {
        switch (_task)
        {
            case SpawnPNJ spawn:
                _positionTracker.UpdatePosition(new GamePosition(spawn.SpawnData.GamePosition.Value));
                break;
            case Move move:
                _positionTracker.UpdatePosition(new GamePosition(move.Destination.Value));
                break;
            default:
                // do nothing
                break;
        }
        OnTaskFinished();
    }

    protected override void RefreshCurrentTaskProgress()
    {
        ProgressPrc = _currentTaskDurationSec / _maxDurationTask;
    }
}
