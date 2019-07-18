using System;
using UnityEngine;

[Serializable]
// Data container for the Collider Scanner results
public class ScanInfo
{
    public ScanData StaticData;
    public ScanData DynamicData;

    public bool IsBlocked => StaticData.IsBlocked || DynamicData.IsBlocked;
}


public class ScanData
{
    public Collider2D Collider { private set; get; }
    public bool IsTrigger => Collider != null && Collider.isTrigger;
    public bool IsBlocked => Collider != null && !Collider.isTrigger;

    public ScanData(Collider2D collider)
    {
        Collider = collider;
    }
}
