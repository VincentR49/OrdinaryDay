using System.Collections;
using UnityEngine;

/// <summary>
/// Holder of dialogue information
/// </summary>
public class SpeakableObject : MonoBehaviour
{
    [SerializeField]
    private DialogueAgentData _dialogueAgentData;
    [SerializeField]
    private InteractibleObject _interactibleObject;

    [Header("Optional")]
    [SerializeField]
    private bool _faceOtherWhileSpeaking;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private WalkManager _walkManager;


    private PlayerDialogueRunner _dialogueRunner;


    private void Awake()
    {
        _interactibleObject.OnInteractionStarted += OnInteractionStarted;
    }


    private void OnDestroy()
    {
        _interactibleObject.OnInteractionStarted -= OnInteractionStarted;
    }


    private void OnInteractionStarted(GameObject interactor)
    {
        StartSpeaking(interactor);
    }


    private void Start()
    {
        _dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // change this later
    }


    public void StartSpeaking(GameObject speaker)
    {
        if (!CanSpeak())
        {
            Debug.LogError("Cannot speak to: " + gameObject.name);
            return;
        }
        AddDialogueDataIfNeeded();
        StartCoroutine(StartSpeakingRoutine(speaker));
    }


    private IEnumerator StartSpeakingRoutine(GameObject speaker)
    {
        if (_walkManager != null) // stop the game object if he is walking (animation bug otherwise)
        {
            _walkManager.Stop();
            yield return new WaitForEndOfFrame();
        }
        if (_faceOtherWhileSpeaking) // more polite
        {
            _spriteDirectioner.FaceTowards(speaker.transform);
            yield return new WaitForEndOfFrame();
        }
        // Start the dialogue
        _dialogueRunner.StartDialogue(_dialogueAgentData.StoryNode);
        yield break;
    }


    public void SetDialogueData(DialogueAgentData dialogueAgentData)
    {
        _dialogueAgentData = dialogueAgentData;
    }


    public bool CanSpeak()
    {
        return ! _dialogueRunner.isDialogueRunning;
    }


    private void AddDialogueDataIfNeeded()
    {
        if (_dialogueAgentData.YarnDialogue != null
                && !_dialogueRunner.NodeExists(_dialogueAgentData.StoryNode))
        {
            Debug.Log("Added script on dialogue runner: " + _dialogueAgentData.StoryNode);
            _dialogueRunner.AddScript(_dialogueAgentData.YarnDialogue);
        }
    }
}
