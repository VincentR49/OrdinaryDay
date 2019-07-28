using UnityEngine;

/// <summary>
/// Generic abstract class for defining a Task
/// </summary>
public abstract class Task : ScriptableObject
{
    public string Description = "";

    public override string ToString() => Description;

    private void OnEnable()
    {
        // Fast way to set the description
        if (string.IsNullOrEmpty(Description))
        {
            Description = name;
        }
    }
}
