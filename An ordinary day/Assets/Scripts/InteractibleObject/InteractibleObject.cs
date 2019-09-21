using UnityEngine;

/// <summary>
/// Attach to an object that can be interactible via InteractWithObjectStarter.
/// Raise an event when the interaction is started.
/// Need a collider to be detected via InteractWithObjectStarter.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class InteractibleObject : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Interaction priority level")]
    private int _priorityLevel;

    [SerializeField]
    [Tooltip("Behaviours should implement I_InteractionResponse")]// todo find a way later to do so
    private MonoBehaviour[] _interactionResponses;


    public void InteractWith(GameObject interactor)
    {
        Debug.Log("Start interaction with: " + gameObject.name);
        if (_interactionResponses == null)
            return;
        foreach (var response in _interactionResponses)
        {
            if (response is I_InteractionResponse)
            {
                var interactionResponse = (I_InteractionResponse) response;
                interactionResponse.OnInteraction(interactor);
            }
            else
            {
                Debug.LogError("The behaviour doesnt implement I_InteractionResponseInterface");
            }
        }
    }

    public int GetPriorityLevel() => _priorityLevel;
}
