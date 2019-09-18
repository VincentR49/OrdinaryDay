using System.Collections;
using UnityEngine;
/// <summary>
/// Attach this to a game object to make him able to speak (start dialogue) with another one.
/// 
/// </summary>
public class SpeakableObject : MonoBehaviour, I_InteractionResponse
{
    [SerializeField]
    private DialogueAgentData _dialogueAgentData;

    [Header("Optional")]
    [SerializeField]
    private bool _faceOtherWhileSpeaking;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private WalkManager _walkManager;

    private PlayerDialogueRunner _dialogueRunner;


    private void Start()
    {
        _dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // change this later
    }


    #region Speaking
    public void SpeaksTo(GameObject other, string nodeTag = null)
    {
        if (!CanSpeak())
        {
            Debug.LogError("Cannot speak to: " + gameObject.name);
            return;
        }
        StartCoroutine(SpeaksToRoutine(other));
    }


    private IEnumerator SpeaksToRoutine(GameObject other, string nodeTag = null)
    {
        if (_walkManager != null) // stop the game object if he is walking (animation bug otherwise)
        {
            _walkManager.Stop();
            yield return new WaitForEndOfFrame();
        }
        if (_faceOtherWhileSpeaking) // more polite
        {
            _spriteDirectioner.FaceTowards(other.transform);
            yield return new WaitForEndOfFrame();
        }
		StartDialogue(nodeTag);
		yield break;
    }
    

    private void StartDialogue(string nodeTag = null)
	{
        var node = _dialogueAgentData.DefaultStoryNode;
        if (!string.IsNullOrEmpty(nodeTag))
		{
            node = _dialogueAgentData.GetNode(nodeTag);
            if (string.IsNullOrEmpty(node))
            {
                Debug.LogError("Couldnt find any node with tag: " + nodeTag);
                return;
            }
		}
        AddDialogueDataIfNeeded(node);
        _dialogueRunner.StartDialogue(node);
	}
    #endregion


    public void SetDialogueData(DialogueAgentData dialogueAgentData)
    {
        _dialogueAgentData = dialogueAgentData;
    }


    private bool CanSpeak() => ! _dialogueRunner.isDialogueRunning;


    private void AddDialogueDataIfNeeded(string node)
    {
        if (_dialogueAgentData.YarnDialogue != null
                && !_dialogueRunner.NodeExists(node)) // TODO throw error if no nodes are loaded, check why
        {
            Debug.Log("Added script on dialogue runner: " + _dialogueAgentData.YarnDialogue + " for node: " + node);
            _dialogueRunner.AddScript(_dialogueAgentData.YarnDialogue);
        }
    }

    public void OnInteraction(GameObject interactor)
    {
        SpeaksTo(interactor);
    }
}
