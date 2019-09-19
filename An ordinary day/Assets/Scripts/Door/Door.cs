using UnityEngine;

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
            if (_locked && inventoryHolder.HasItem(_key.Tag))
            {
                nodeTag = UnlockChoiceNodeTag;
            }
        }
        _speakableObject.SpeaksTo(interactor, nodeTag);
        //TODO Add listener to the speakable object to check when the dialogue is finished
    }


    private void OnDialogueFinished(string lastYarnNode)
    {
        // We unlock the door if the last dialogue node is the justUnlocked node.
        var nodeTag = _speakableObject.GetNodeTag(lastYarnNode);
        if (nodeTag.Equals(JustUnlockedNodeTag))
        {
            Unlock();
        }
    }


    public void Lock()
    {
        SetLocked(true);
    }

    
    public void Unlock()
    {
        SetLocked(false);
    }

    private void SetLocked(bool locked)
    {
        _locked = locked;
        _playerTeleporterObject.SetActive(!locked);
    }
}
