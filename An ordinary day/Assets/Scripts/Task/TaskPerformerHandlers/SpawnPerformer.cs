using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Performer that enable to spawn a game object in the current scene
/// The spawn point should be present in the scene to succeed.
/// </summary>
public class SpawnPerformer : TaskPerformerHandler
{
    private GameObject _goToSpawn;
    private SpawnPoint _currentSpawnPoint;

    public void Perform(Spawn spawn, SpawnPointList spawnList, GameObject go, List<MonoBehaviour> disableDuringSpawn = null)
    {
        base.Perform(spawn);
        _goToSpawn = go;
        if (!spawn.IsInCurrentScene())
        {
            OnTaskFailed(TaskFailedConstants.NotInGoodScene);
            return;
        }
        var spawnPoint = spawnList.GetSpawnPoint(spawn.SpawnPointTag);
        if (spawnPoint != null)
        {
            _currentSpawnPoint = spawnPoint;
            _currentSpawnPoint.OnSpawnFinished += OnSpawnFinished;
            _currentSpawnPoint.Spawn(go, disableDuringSpawn);
        }
        else
        {
            OnTaskFailed(TaskFailedConstants.SpawnPointNotFound, spawn.SpawnPointTag);
        }
    }


    private void OnSpawnFinished(GameObject go)
    {
        if (go == _goToSpawn)
            OnTaskFinished();
    }

    protected override void Clean()
    {
        _currentSpawnPoint.OnSpawnFinished -= OnSpawnFinished;
        base.Clean();
    }
}
