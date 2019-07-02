using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataList<T> : ScriptableObject
{
    protected List<T> _items;


    public void Add(T item)
    {
        if (_items == null)
        {
            _items = new List<T>();
        }
        _items.Add(item);
    }


    public void Remove(T item)
    {
        if (_items == null)
            return;
        _items.Remove(item);
        if (_items.Count == 0)
            _items = null;
    }


    public void Clear()
    {
        if (_items == null)
            return;
        _items.Clear();
        _items = null;
    }
}
