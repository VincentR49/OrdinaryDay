using UnityEngine;

/// <summary>
/// Behaviour that enables to start an interaction with a given interactible object
/// </summary>
public class InteractWithObjectStarter : MonoBehaviour
{
    [SerializeField]
    private float _interactionRadius;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;

    private Direction Direction => _spriteDirectioner.GetDirection();

    
    public bool StartInteractionIfPossible()
    {
        if (GamePauser.IsPaused)
            return false;
        var obj = CheckForNearbyObjects();
        if (obj == null)
            return false;
        StartInteraction(obj);
        return true;
    }


    private void StartInteraction(InteractibleObject obj)
    {
        obj.InteractWith(gameObject);
    }


    private InteractibleObject CheckForNearbyObjects()
    {
        Debug.Log("CheckForNearbyObjects");
        var nearbyColliders = ScanCollidersNearby();
        if (nearbyColliders == null)
            return null;
        InteractibleObject objCandidate = null;
        foreach (var coll in nearbyColliders)
        {
            var intObj = coll.GetComponent<InteractibleObject>();
            // need to face the object to interact with it
            if (intObj != null && IsFacingObject(intObj.transform))
            {
                if (objCandidate == null
                        || (objCandidate.GetPriorityLevel() < intObj.GetPriorityLevel()))
                    objCandidate = intObj;
            }   
        }
        return objCandidate;
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
