using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RuntimeVariable/PositionTrackingData")]
public class PositionTrackingData : ScriptableObject
{
    [NonSerialized]
    public DatedVariable<GamePosition> LastPosition;
    [NonSerialized]
    public DatedVariable<SpawnData> LastSpawn;

    public PositionTrackingData()
    {
        LastPosition = new DatedVariable<GamePosition>();
        LastPosition.Value = new GamePosition();
        LastSpawn = new DatedVariable<SpawnData>();
    }
}
