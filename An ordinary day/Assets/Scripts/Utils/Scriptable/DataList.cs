using System.Collections.Generic;
using UnityEngine;

public class DataList<T> : ScriptableObject
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
}
