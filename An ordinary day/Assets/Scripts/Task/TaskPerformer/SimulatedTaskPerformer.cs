using UnityEngine;

/// <summary>
/// Simulated task Performer.
/// Basic TaskPerformer that ends automatically a task after reaching the defined duration.
/// </summary>
public class SimulatedTaskPerformer : BasicTaskPerformer
{
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
        var pnj = PNJInstancier.InstanciatePNJ(spawn.PNJ, spawn.SpawnData);
        if (pnj == null)
        {
            OnTaskFailed(TaskFailedConstants.SpawnPointNotFound); // we skip the task (succeed by default, simulation)
            return;
        }
        _positionTracker.UpdatePosition(new GamePosition(spawn.SpawnData.GamePosition.Value));
        OnTaskFinished();
    }
    #endregion

    // Finish task when we reach the limit duration
    protected override void OnCurrentTaskDurationReachedLimit()
    {
        OnTaskFinished();
    }

    protected override void RefreshCurrentTaskProgress()
    {
        ProgressPrc = _currentTaskDurationSec / _maxDurationTask;
    }
}
