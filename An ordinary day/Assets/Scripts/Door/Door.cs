using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach this to a game object simulate a door behaviour.
/// </summary>
public class Door : MonoBehaviour, I_InteractionResponse
{
    [Header("Necessary Behaviours")]
    [SerializeField]
    private DialogueWithPlayerAgent _speakableObject;
    [SerializeField]
    private GameObject _playerTeleporterObject;

    [SerializeField]
    [Tooltip("Optional")]
    private GameItemData _key;
    [SerializeField]
    private bool _locked;

    
    private DoorDialogueData DialogueData => (DoorDialogueData) _speakableObject.GetDialogueData();


    private void Awake()
    {
        SetLocked(_locked);
    }


    public void OnInteraction(GameObject interactor)
    {
        var inventoryHolder = interactor.GetComponent<InventoryHolder>();
        var node = _locked ? DialogueData.LockedNode : DialogueData.UnlockedNode;
        if (inventoryHolder == null)
        {
            Debug.LogWarning("The interactor doesnt have any inventory system.");
        }
        else // has inventory
        {
            if (_locked && _key != null && inventoryHolder.HasItem(_key.Tag))
            {
                node = DialogueData.UnlockChoiceNode;
            }
        }
        _speakableObject.OnDialogueFinished += OnDialogueFinished;
        _speakableObject.SpeaksTo(interactor, node);
    }


    private void OnDialogueFinished(List<string> visitedNodes)
    {
        // We unlock the door if the player has visited the just unlocked node
        _speakableObject.OnDialogueFinished -= OnDialogueFinished;
        foreach (var node in visitedNodes)
        {
            if (DialogueData.JustUnlockedNode.Equals(node))
            {
                Unlock();
            }
        }
    }


    public void Lock()
    {
        Debug.Log("Lock Door " + gameObject.name);
        SetLocked(true);
    }

    
    public void Unlock()
    {
        Debug.Log("Unlock Door " + gameObject.name);
        SetLocked(false);
    }


    private void SetLocked(bool locked)
    {
        _locked = locked;
        _playerTeleporterObject.SetActive(!locked);
    }
}
