using UnityEngine;

/// <summary>
/// Attach to an object that can be interactible via InteractWithObjectStarter.
/// Raise an event when the interaction is started.
/// </summary>
public class InteractibleObject : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Interaction priority level")]
    private int _priorityLevel;

    public delegate void InteractionStartedHandler(GameObject interactor);
    public event InteractionStartedHandler OnInteractionStarted;

    public void InteractWith(GameObject interactor)
    {
        Debug.Log("Start interaction with: " + gameObject.name);
        OnInteractionStarted?.Invoke(interactor);
    }

    public int GetPriorityLevel() => _priorityLevel;
}
