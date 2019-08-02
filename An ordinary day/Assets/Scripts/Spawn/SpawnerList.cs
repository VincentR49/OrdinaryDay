using UnityEngine;

[CreateAssetMenu(menuName ="GameData/SpawnerList")]
public class SpawnerList : DataList<Spawner>
{
    public Spawner GetSpawner(SpawnData spawnData)
    {
        if (Items == null || spawnData == null)
            return null;
        foreach (var spawner in Items)
        {
            if (spawner.GetSpawnData() == spawnData)
                return spawner;
        }
        return null;
    }
}
