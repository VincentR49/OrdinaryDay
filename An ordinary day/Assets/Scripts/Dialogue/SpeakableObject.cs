using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Attach this to a game object to make him able to speak (start dialogue) with another one.
/// 
/// </summary>
public class SpeakableObject : MonoBehaviour, I_InteractionResponse
{
    private const float ListenDialogueRoutineRefreshRateSec = 0.25f;
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

    public delegate void DialogueFinishedHandler(List<string> _visitedNodes);
    public event DialogueFinishedHandler OnDialogueFinished;


    private void Start()
    {
        _dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // change this later
    }


    #region Speaking
    public void SpeaksTo(GameObject other, string nodeTag = null, bool tagIsYarnNodeName = false)
    {
        if (!CanSpeak())
        {
            Debug.LogError("Cannot speak to: " + gameObject.name);
            return;
        }
        StartCoroutine(SpeaksToRoutine(other, nodeTag, tagIsYarnNodeName));
    }


    private IEnumerator SpeaksToRoutine(GameObject other, string nodeTag = null, bool tagIsYarnNodeName = false)
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
		StartDialogue(nodeTag, tagIsYarnNodeName);
		yield break;
    }
    

    private void StartDialogue(string nodeTag = null, bool tagIsYarnNodeName = false)
	{
        var node = _dialogueAgentData.DefaultStoryNode;
        if (!string.IsNullOrEmpty(nodeTag))
		{
            node = tagIsYarnNodeName ? nodeTag : _dialogueAgentData.GetYarnNode(nodeTag);
            if (string.IsNullOrEmpty(node))
            {
                Debug.LogError("Couldnt find any node with tag: " + nodeTag);
                return;
            }
		}
        AddDialogueDataIfNeeded(node);
        _dialogueRunner.StartDialogue(node);
        StartCoroutine(ListenForEndOfDialogue());
	}


    private IEnumerator ListenForEndOfDialogue()
    {
        var visitedNodes = new List<string>();
        yield return new WaitForSecondsRealtime(ListenDialogueRoutineRefreshRateSec);
        while (_dialogueRunner.isDialogueRunning)
        {
            var currentNode = _dialogueRunner.currentNodeName;
            if (!string.IsNullOrEmpty(currentNode)
                    && !visitedNodes.Contains(currentNode))
            {
                visitedNodes.Add(currentNode);
            }
            yield return new WaitForSecondsRealtime(ListenDialogueRoutineRefreshRateSec);
        }
        Debug.Log("Visited nodes: " + string.Join(", ", visitedNodes));
        OnDialogueFinished?.Invoke(visitedNodes);
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


    public string GetYarnNodeName(string nodeTag)
    {
        return _dialogueAgentData.GetYarnNode(nodeTag);
    }


    public string GetNodeTag(string yarnNodeName)
    {
        return _dialogueAgentData.GetNodeTag(yarnNodeName);
    }
}
