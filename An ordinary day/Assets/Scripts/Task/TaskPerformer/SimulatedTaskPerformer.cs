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
            case SpawnNPC spawn:
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
    public void PerformSpawn(SpawnNPC spawn)
    {
        OnTaskFinished(); // automatic success
        if (spawn.IsInCurrentScene())
        {
            // we instanciate the npc if needed and make it spawn at the good position
            NPCInstancier.InstanciateNPC(spawn.NPC, spawn.SpawnData);
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
