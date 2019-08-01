using UnityEngine;

[CreateAssetMenu(menuName ="GameData/SpawnPointList")]
public class SpawnPointList : DataList<SpawnPoint>
{
    public SpawnPoint GetSpawnPoint(string tag)
    {
        if (Items != null)
        {
            foreach (var spawnPoint in Items)
            {
                if (spawnPoint.GetTag().Equals(tag))
                    return spawnPoint;
            }
        }
        return null;
    }
}
