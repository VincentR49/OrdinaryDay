using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton to instanciate PNJ
/// </summary>
public class PNJInstancier : Singleton<PNJInstancier>
{
    private static PNJController _pnjPrefab =
        Utils.GetPrefab(PathConstants.PNJPrefab).GetComponent<PNJController>();


    public static PNJController InstanciatePNJ(PNJData pnjData, SpawnData spawnData)
    {
        var pnj = Instance.InstanciateIfNeeded(pnjData);
        Spawner.Spawn(pnj.gameObject, spawnData, new List<MonoBehaviour>
        {
            pnj.GetComponent<ScheduleHandler>()
        });
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
