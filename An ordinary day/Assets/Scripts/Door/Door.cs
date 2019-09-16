using UnityEngine;

/// <summary>
/// Attach this to a game object simulate a door behaviour.
/// </summary>
public class Door : MonoBehaviour
{
    private const string LockedNode = "Locked";
    private const string UnlockedNode = "Unlocked";

    [SerializeField]
    private GameItemData _key;
    [SerializeField]
    private bool _isLockedByDefault;
    [SerializeField]
    private bool _autoTriggerOnInteraction = true;

    [Header("Necessary Behaviours")]
    [SerializeField]
    private SpeakableObject _speakableObject;
    [SerializeField]
    private InteractibleObject _interactibleObject;
    [SerializeField]
    private GameObject _playerTeleporterObject;


    private bool _locked;


    private void Awake()
    {
        _interactibleObject.OnInteractionStarted += OnInteractionStarted;
        SetLocked(_isLockedByDefault ? true : false);
    }


    private void OnDestroy()
    {
        _interactibleObject.OnInteractionStarted -= OnInteractionStarted;
    }


    private void OnInteractionStarted(GameObject interactor)
    {
        if (_autoTriggerOnInteraction)
        {
            _speakableObject.StartDialogue(_locked ? LockedNode : UnlockedNode);
        }
    }


    private void SetLocked(bool locked)
    {
        _locked = locked;
        _playerTeleporterObject.SetActive(!locked);
    }
}
