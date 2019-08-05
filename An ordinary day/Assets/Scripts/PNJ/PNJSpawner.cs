using System.Collections.Generic;
using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    [SerializeField]
    private PNJController _pnjPrefab;
    [SerializeField]
    private PNJControllerList _instanciatedPNJs;

    public PNJController Spawn(PNJData pnjData, SpawnData spawnData)
    {
        var pnj = InstanciateIfNeeded(pnjData);
        Spawner.Spawn(pnj.gameObject, spawnData, new List <MonoBehaviour>
        {
            pnj.GetComponent<ScheduleHandler>()
        });
        return pnj;
    }


    private PNJController InstanciateIfNeeded(PNJData pnjData)
    {
        var pnj = _instanciatedPNJs.Get(pnjData);
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
