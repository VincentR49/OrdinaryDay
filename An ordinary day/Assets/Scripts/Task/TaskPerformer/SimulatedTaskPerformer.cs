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
        OnTaskFinished(); // automatic success
        if (spawn.IsInCurrentScene())
        {
            // we instanciate the pnj if needed and make it spawn at the good position
            PNJInstancier.InstanciatePNJ(spawn.PNJ, spawn.SpawnData);
        }
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
