using UnityEngine;

/// <summary>
/// Generic abstract class for defining a Task
/// </summary>
public abstract class Task : ScriptableObject
{
    public string Description = "";

    public override string ToString() => string.IsNullOrEmpty(Description) ? name : Description;
}
