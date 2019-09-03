using UnityEngine;
using System.Collections.Generic;

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
    private List<InteractibleObject> InstanciatedObjects => InteractibleObject.GetInteractibleObjects();
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
        if (InstanciatedObjects == null)
            return null;
        // Better to check on the colliders around maybe??
        foreach (var obj in InstanciatedObjects)
        {
            // TODO need to face the object
            if (Utils.Distance(transform.position, obj.transform.position) < _interactionRadius)
                return obj;
        }
        return null;
    }
}
