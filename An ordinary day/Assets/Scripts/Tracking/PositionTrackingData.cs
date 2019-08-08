using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RuntimeVariable/PositionTrackingData")]
public class PositionTrackingData : ScriptableObject
{
    [NonSerialized]
    public GamePosition GamePosition;

    public PositionTrackingData()
    {
        GamePosition = new GamePosition();
    }
}
