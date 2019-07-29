using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    [SerializeField]
    private PNJController _pnjPrefab;
    [SerializeField]
    private SpawnPointList _spawnPointList;
    [SerializeField]
    private PNJControllerList _instanciatedPNJ;


    public void Spawn(PNJData pnjData, Vector2 position, Direction direction)
    {
        Debug.Log("[PNJSpawner] Created " + pnjData + " at " + position);
        var pnj = InstanciateIfNeeded(pnjData);
        pnj.gameObject.transform.position = position;
        pnj.GetComponent<SpriteDirectioner>().SetSprite(direction);
    }


    public void Spawn(PNJData pnjData, string spawnTag)
    {
        var spawn = _spawnPointList.GetSpawnPoint(spawnTag);
        if (spawn != null)
        {
            var pnj = InstanciateIfNeeded(pnjData);
            spawn.Spawn(pnj.gameObject);
        }
        else
        {
            Debug.LogError("Couldnt find spawn point of tag: " + spawnTag);
        }
    }


    private PNJController InstanciateIfNeeded(PNJData pnjData)
    {
        var pnj = _instanciatedPNJ.Get(pnjData);
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
