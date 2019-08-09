using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Singleton to instanciate PNJ
/// </summary>
public class PNJInstancier : Singleton<PNJInstancier>
{
    private static PNJController _pnjPrefab =
        Utils.GetPrefab(PathConstants.PNJPrefab).GetComponent<PNJController>();


    public static PNJController InstanciatePNJ(PNJData pnjData, SpawnData spawnData, Action executeAfterSpawn = null)
    {
        if (!spawnData.IsInCurrentScene())
        {
            Debug.LogError("Spawn should be in different scene.");
            return null;
        }
        return InstanciatePNJ(pnjData, spawnData.Position, spawnData.Direction, executeAfterSpawn);
    }

    
    public static PNJController InstanciatePNJ(PNJData pnjData, Vector2 position, Direction direction, Action executeAfterSpawn = null)
    {
        var pnj = Instance.InstanciateIfNeeded(pnjData);
        Spawner.Spawn(pnj.gameObject, position, direction, new List<MonoBehaviour>
        {
            pnj.GetComponent<ScheduleHandler>()
        }, executeAfterSpawn);
        return pnj;
    }


    private PNJController InstanciateIfNeeded(PNJData pnjData)
    {
        var pnj = PNJController.Get(pnjData);
        if (pnj == null) // if not in current scene, make it spawn
            pnj = Instantiate(_pnjPrefab);
        InitPNJ(pnj, pnjData);
        return pnj;
    }


    private void InitPNJ(PNJController pnj, PNJData pnjData)
    {
        pnj.Init(pnjData);
    }
}
