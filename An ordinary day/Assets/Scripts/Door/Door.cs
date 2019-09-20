using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach this to a game object simulate a door behaviour.
/// </summary>
public class Door : MonoBehaviour, I_InteractionResponse
{
    // Dialogue Agent Data Node tag corresponding the Lock and Unlock text to display
    private const string LockedNodeTag = "Locked";
    private const string UnlockedNodeTag = "Unlocked";
    private const string UnlockChoiceNodeTag = "UnlockChoice";
    private const string JustUnlockedNodeTag = "JustUnlocked";

    [SerializeField]
    [Tooltip("Optional")]
    private GameItemData _key;
    [SerializeField]
    private bool _locked;

    [Header("Necessary Behaviours")]
    [SerializeField]
    private SpeakableObject _speakableObject;
    [SerializeField]
    private GameObject _playerTeleporterObject;


    private void Awake()
    {
        SetLocked(_locked);
    }


    public void OnInteraction(GameObject interactor)
    {
        // TODO Manage here the game logic
        var inventoryHolder = interactor.GetComponent<InventoryHolder>();
        var nodeTag = _locked ? LockedNodeTag : UnlockedNodeTag;
        if (inventoryHolder == null)
        {
            Debug.LogWarning("The interactor doesnt have any inventory system.");
        }
        else // has inventory
        {
            if (_locked && _key != null && inventoryHolder.HasItem(_key.Tag))
            {
                nodeTag = UnlockChoiceNodeTag;
            }
        }
        _speakableObject.OnDialogueFinished += OnDialogueFinished;
        _speakableObject.SpeaksTo(interactor, nodeTag);
    }


    private void OnDialogueFinished(List<string> visitedNodes)
    {
        // We unlock the door if the player has visited the just unlocked node
        _speakableObject.OnDialogueFinished -= OnDialogueFinished;
        foreach (var node in visitedNodes)
        {
            var nodeTag = _speakableObject.GetNodeTag(node);
            Debug.Log(nodeTag);
            if (JustUnlockedNodeTag.Equals(nodeTag))
            {
                Unlock();
            }
        }
    }


    public void Lock()
    {
        Debug.Log("Lock " + gameObject.name);
        SetLocked(true);
    }

    
    public void Unlock()
    {
        Debug.Log("Unlock " + gameObject.name);
        SetLocked(false);
    }

    private void SetLocked(bool locked)
    {
        _locked = locked;
        _playerTeleporterObject.SetActive(!locked);
    }
}
