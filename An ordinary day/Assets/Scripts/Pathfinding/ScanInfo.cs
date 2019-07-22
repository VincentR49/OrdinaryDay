using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// Data container for the Collider Scanner results
public class ScanInfo
{
    private Dictionary<LayerMask, Collider2D> _scanDataDict;

    public ScanInfo()
    {
        _scanDataDict = new Dictionary<LayerMask, Collider2D>();
    }

    public void Set(LayerMask layerMask, Collider2D collider)
    {
        _scanDataDict[layerMask] = collider;
    }

    public Collider2D Get(LayerMask layerMask)
    {
        if (_scanDataDict.ContainsKey(layerMask))
            return _scanDataDict[layerMask];
        return null;
    }

    /// <summary>
    /// Return true if the layer mask contains a trigger
    /// </summary>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public bool HasTrigger(LayerMask layerMask)
    {
        var collider = Get(layerMask);
        return collider != null && collider.isTrigger;
    }

    /// <summary>
    /// Return true if the layerMask contains a collider which is not a trigger
    /// </summary>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public bool HasSolidCollider(LayerMask layerMask)
    {
        var collider = Get(layerMask);
        return collider != null && !collider.isTrigger;
    }


    public bool HasSolidCollider()
    {
        var hasCollider = false;
        foreach (var key in _scanDataDict.Keys)
            hasCollider = hasCollider || HasSolidCollider(key);
        return hasCollider;
    }
}
