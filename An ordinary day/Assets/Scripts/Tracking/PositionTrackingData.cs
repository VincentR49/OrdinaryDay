using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RuntimeVariable/PositionTrackingData")]
public class PositionTrackingData : ScriptableObject
{
    [NonSerialized]
    public GamePosition LastPosition;

    public PositionTrackingData()
    {
        LastPosition = new GamePosition();
    }
}
