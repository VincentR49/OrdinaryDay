using System.Collections.Generic;
using UnityEngine;


public class VariableDataList<T> : ScriptableObject
{
    public List<T> List;

    public void Add(T item)
    {
        if (List == null)
            List = new List<T>();
        List.Add(item);
    }

    public void Remove(T item)
    {
        if (List == null)
            return;
        List.Remove(item);
    }


    public T Find(T item)
    {
        if (List == null)
            return default;
        foreach (var t in List)
        {
            if (t.Equals(item))
                return t;
        }
        return default;
    }
}
