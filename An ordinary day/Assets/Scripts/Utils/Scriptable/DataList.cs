using System.Collections.Generic;
using UnityEngine;

public class DataList<T> : ScriptableObject where T: UnityEngine.Object
{
    public List<T> Items;
    
    public void Add(T item)
    {
        if (Items == null)
        {
            Items = new List<T>();
        }
        if (Items.Contains(item))
            return;
        Items.Add(item);
    }


    public void Remove(T item)
    {
        if (Items == null)
            return;
        Items.Remove(item);
        if (Items.Count == 0)
            Items = null;
    }


    public void Clear()
    {
        if (Items == null)
            return;
        Items.Clear();
    }


    public void Add(List<T> items)
    {
        foreach (var item in items)
            Add(item);
    }


    public void LoadAllFromPath(string folder)
    {
        var assets = Utils.FindAssetsByType<T>(folder);
        Clear();
        Add(assets);
    }
}
