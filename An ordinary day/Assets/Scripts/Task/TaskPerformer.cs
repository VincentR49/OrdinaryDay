using UnityEngine;

/// <summary>
/// Behaviour that handle task
/// </summary>
public class TaskPerformer : MonoBehaviour
{
    public void Perform(Task task)
    {
        Debug.Log("Perform " + task.Description);
    }
}
