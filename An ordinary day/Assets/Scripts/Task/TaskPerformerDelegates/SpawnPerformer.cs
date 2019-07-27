using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Task that enable to spawn a go in the current scene
/// The spawn point should be present in the scene to succeed.
/// </summary>
public class SpawnPerformer : TaskPerformerDelegate
{
    public void Perform(Spawn spawn, SpawnPointList spawnList, GameObject go, List<MonoBehaviour> disableDuringSpawn = null)
    {
        if (!spawn.IsInCurrentScene())
        {
            OnTaskFailed(TaskFailedConstants.NotInGoodScene);
            return;
        }
        var spawnPoint = spawnList.GetSpawnPoint(spawn.SpawnPointTag);
        if (spawnPoint != null)
        {
            spawnPoint.Spawn(go, disableDuringSpawn);
            OnTaskFinished();
        }
        else
        {
            OnTaskFailed(TaskFailedConstants.SpawnPointNotFound, spawn.SpawnPointTag);
        }
    }
}
