using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Attach this to a game object simulate a door behaviour.
/// </summary>
public class Door : MonoBehaviour, I_InteractionResponse
{
    // Dialogue Agent Data Node tag corresponding the Lock and Unlock text to display
    private const string LockedNodeTag = "Locked";
    private const string UnlockedNodeTag = "Unlocked";
    
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
        _speakableObject.SpeaksTo(interactor, _locked ? LockedNodeTag : UnlockedNodeTag);
    }

    [YarnCommand("lock")]
    public void Lock()
    {
        SetLocked(true);
    }

    [YarnCommand("unlock")]
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
