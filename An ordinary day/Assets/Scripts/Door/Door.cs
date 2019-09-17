using UnityEngine;

/// <summary>
/// Attach this to a game object simulate a door behaviour.
/// </summary>
public class Door : MonoBehaviour, I_InteractionResponse
{
    // Yarn Node corresponding the Lock and Unlock text to display
    private const string LockedNode = "Locked";
    private const string UnlockedNode = "Unlocked";

    [SerializeField]
    private GameItemData _key;
    [SerializeField]
    private bool _isLockedByDefault;

    [Header("Necessary Behaviours")]
    [SerializeField]
    private SpeakableObject _speakableObject;
    [SerializeField]
    private GameObject _playerTeleporterObject;

    private bool _locked;


    private void Awake()
    {
        SetLocked(_isLockedByDefault ? true : false);
    }


    public void OnInteraction(GameObject interactor)
    {
        _speakableObject.SpeaksTo(interactor, _locked ? LockedNode : UnlockedNode);
    }


    private void SetLocked(bool locked)
    {
        _locked = locked;
        _playerTeleporterObject.SetActive(!locked);
    }
}
