using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject
{
    protected List<T> set = new List<T>();

    private void OnEnable()
    {
        set.Clear();
    }

    public T this[int index]
    {
        get { return set[index]; }
        set { set[index] = value; }
    }

    public void Add(T item)
    {
        set.Add(item);
    }

    public void Remove(T item)
    {
        set.Remove(item);
    }
}