using System.Collections;
using UnityEngine;
/// <summary>
/// Attach this to a game object to make him able to speak (start dialogue) with another one.
/// 
/// </summary>
public class SpeakableObject : MonoBehaviour
{
    [SerializeField]
    private DialogueAgentData _dialogueAgentData;
    [SerializeField]
    private InteractibleObject _interactibleObject;
    [SerializeField]
    private bool _autoSpeakingOnInteraction = true;

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

    private void Start()
    {
        _dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // change this later
    }

    private void OnDestroy()
    {
        _interactibleObject.OnInteractionStarted -= OnInteractionStarted;
    }


    private void OnInteractionStarted(GameObject interactor)
    {
        if (_autoSpeakingOnInteraction)
        {
            StartSpeaking(interactor);
        }
    }


    public void StartSpeaking(GameObject speaker)
    {
        if (!CanSpeak())
        {
            Debug.LogError("Cannot speak to: " + gameObject.name);
            return;
        }
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
		StartDialogue();
		yield break;
    }


    public void StartDialogue(string nodeTag = null)
	{
        AddDialogueDataIfNeeded();
        var node = _dialogueAgentData.DefaultStoryNode;
        if (!string.IsNullOrEmpty(nodeTag))
		{
            node = _dialogueAgentData.GetNode(nodeTag);
		}
		_dialogueRunner.StartDialogue(node);
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
                && !_dialogueRunner.NodeExists(_dialogueAgentData.DefaultStoryNode))
        {
            Debug.Log("Added script on dialogue runner: " + _dialogueAgentData.DefaultStoryNode);
            _dialogueRunner.AddScript(_dialogueAgentData.YarnDialogue);
        }
    }
}
