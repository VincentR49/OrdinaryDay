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
}
