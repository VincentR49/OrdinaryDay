using UnityEngine;

/// <summary>
/// Behaviour that enables to start an interaction with a given interactible objects
/// </summary>
public class InteractWithObjectStarter : MonoBehaviour
{
    [SerializeField]
    private float _interactionRadius;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;

    private PlayerDialogueRunner _dialogueRunner;
    private Direction Direction => _spriteDirectioner.GetDirection();

    private void Start()
    {
        _dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // change this later
    }


    public bool StartInteractionIfPossible()
    { 
        if (_dialogueRunner.isDialogueRunning)
            return false;
        var obj = CheckForNearbyObjects();
        if (obj == null)
            return false;
        StartInteraction(obj);
        return true;
    }


    public void StartInteraction(InteractibleObject obj)
    {
        _dialogueRunner.StartDialogue(obj.GetData().StartNodeStory);
    }


    public InteractibleObject CheckForNearbyObjects()
    {
        Debug.Log("CheckForNearbyObjects");
        var nearbyColliders = ScanCollidersNearby();
        if (nearbyColliders == null)
            return null;
        foreach (var coll in nearbyColliders)
        {
            var intObj = coll.GetComponent<InteractibleObject>();
            if (intObj != null)
            {
                // the current caracter should face the object in order to interact with it
                if (IsFacingObject(intObj.transform))
                {
                    return intObj;
                }
            }   
        }
        return null;
    }


    private Collider2D[] ScanCollidersNearby()
    {
        return Physics2D.OverlapCircleAll(transform.position, _interactionRadius);
    }


    private bool IsFacingObject(Transform otherTransform)
    {
        var directionToObject = Utils.GetDirection(otherTransform.position - transform.position);
        Debug.Log("Direction to object: " + directionToObject);
        return directionToObject == Direction;
    }
}
