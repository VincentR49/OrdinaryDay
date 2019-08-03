using UnityEngine;

/// <summary>
/// Simulated task Performer.
/// Basic TaskPerformer that ends automatically a task after reaching the defined duration.
/// </summary>
public class SimulatedTaskPerformer : BasicTaskPerformer
{
    [SerializeField]
    private PNJSpawner _pnjSpawner;


    public override void Perform(Task task, float maxDurationSec, float initProgressPrc)
    {
        base.Perform(task, maxDurationSec, initProgressPrc);
        switch (task)
        {
            case SpawnPNJ spawn:
                PerformSpawn(spawn);
                break;
            default:
                Debug.LogError("Task not implemented yet for simulation: " + task);
                break;
        }
    }

    #region Spawn
    public void PerformSpawn(SpawnPNJ spawn)
    {
        var success = _pnjSpawner.Spawn(spawn.PNJ, spawn.SpawnData);
        if (!success)
        {
            OnTaskFailed(TaskFailedConstants.SpawnPointNotFound); // we skip the task (succeed by default, simulation)
            return;
        }
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
