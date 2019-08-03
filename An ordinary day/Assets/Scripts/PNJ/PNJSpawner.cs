using System.Collections.Generic;
using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    [SerializeField]
    private PNJController _pnjPrefab;
    [SerializeField]
    private SpawnerList _spawnerList;
    [SerializeField]
    private PNJControllerList _instanciatedPNJs;

    public bool Spawn(PNJData pnjData, SpawnData spawnData)
    {
        var spawn = _spawnerList.GetSpawner(spawnData);
        if (spawn == null)
        {
            Debug.LogError("Couldnt find spawn point of tag: " + spawnData);
            return false;
        }
        var pnj = InstanciateIfNeeded(pnjData);
        spawn.Spawn(pnj.gameObject, new List<MonoBehaviour>
        {
            pnj.GetComponent<ScheduleHandler>()
        });
        return true;
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
