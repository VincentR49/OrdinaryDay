using UnityEngine;

[CreateAssetMenu(menuName ="GameData/SpawnPointList")]
public class SpawnPointList : DataList<SpawnPoint>
{
    public SpawnPoint GetSpawnPoint(string tag)
    {
        if (_items != null)
        {
            foreach (var spawnPoint in _items)
            {
                if (spawnPoint.GetTag().Equals(tag))
                    return spawnPoint;
            }
        }
        return null;
    }
}
