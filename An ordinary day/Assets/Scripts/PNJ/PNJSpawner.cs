using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    [SerializeField]
    private PNJController _pnjPrefab;
    [SerializeField]
    private SpawnerList _spawnerList;
    [SerializeField]
    private PNJControllerList _instanciatedPNJs;

    public (PNJController, Spawner) Spawn(PNJData pnjData, SpawnData spawnData)
    {
        var spawn = _spawnerList.GetSpawner(spawnData);
        if (spawn == null)
        {
            Debug.LogError("Couldnt find spawn point of tag: " + spawnData);
            return (null,null);
        }
        var pnj = InstanciateIfNeeded(pnjData);
        spawn.Spawn(pnj.gameObject);
        return (pnj, spawn);
    }


    private PNJController InstanciateIfNeeded(PNJData pnjData)
    {
        var pnj = _instanciatedPNJs.Get(pnjData);
        if (pnj == null) // if not in current scene, make it spawn
            pnj = Instanciate(pnjData);
        return pnj;
    }


    private PNJController Instanciate(PNJData pnjData)
    {
        var pnj = Instantiate(_pnjPrefab);
        pnj.Init(pnjData);
        return pnj;
    }
}
