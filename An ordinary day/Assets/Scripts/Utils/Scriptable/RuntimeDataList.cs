using System.Collections.Generic;
using UnityEngine;
using System;

public class RuntimeDataList<T> : ScriptableObject
{
    public delegate void OnListChanged(T item);
    public event OnListChanged OnItemAdded;
    public event OnListChanged OnItemRemoved;

    [NonSerialized]
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
        OnItemAdded?.Invoke(item);
        Debug.Log("DataList: " + name + ". On item added " + item);
    }


    public void Remove(T item)
    {
        if (Items == null)
            return;
        Items.Remove(item);
        if (Items.Count == 0)
            Items = null;
        OnItemRemoved?.Invoke(item);
        Debug.Log("DataList: " + name + ". On item removed " + item);
    }

    public void Clear()
    {
        if (Items == null)
            return;
        for (int i = Items.Count - 1; i >= 0; i--)
            Remove(Items[i]); // to call the event
    }
}
