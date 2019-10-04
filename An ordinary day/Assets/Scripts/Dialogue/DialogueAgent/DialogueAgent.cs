using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Base class for dialogue agent behaviour.
/// Attach this to a game object to make it able to speaks via Yarn dialogue files.
/// Dialogue agent is related to a DialogueRunnerType file (change this later) // todo
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DialogueAgent<T> : MonoBehaviour where T : DialogueRunner
{
    private const float ListenDialogueRoutineRefreshRateSec = 0.25f;
    [SerializeField]
    protected DialogueAgentData _dialogueAgentData;

    protected T _dialogueRunner;
    public delegate void DialogueFinishedHandler(List<string> _visitedNodes);
    public event DialogueFinishedHandler OnDialogueFinished;


    public void SetDialogueRunner(T dialogueRunner)
    {
        _dialogueRunner = dialogueRunner;
    }

    #region Speaking
    /// <summary>
    /// Speaks to the given game object.
    /// If the nodeTag is not precised, the DefaultStoryNode will be loaded by default.
    /// If yarnFile is not precised, the DefaultDialogueFile will be loaded by default.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="yarnFile"></param>
    public void SpeaksTo(GameObject other, string node = null, TextAsset yarnFile = null)
    {
        if (!CanSpeak())
        {
            Debug.LogError("Cannot speak to: " + gameObject.name);
            return;
        }
        if (node == null)
        {
            node = _dialogueAgentData.DefaultStoryNode;
        }
        if (yarnFile == null)
        {
            yarnFile = _dialogueAgentData.DefaultDialogueFile;
        }
        StartCoroutine(SpeaksToRoutine(other, node, yarnFile));
    }


    public void SpeaksTo(GameObject other, TaggedDialogueNode taggedNode, TextAsset yarnFile = null)
    {
        var node = _dialogueAgentData.GetYarnNode(taggedNode.Tag);
        if (string.IsNullOrEmpty(node))
        {
            Debug.LogError("Couldnt find any node with tag: " + taggedNode.Tag);
            return;
        }
        SpeaksTo(other, node, yarnFile);
    }


    protected virtual IEnumerator SpeaksToRoutine(GameObject other, string node, TextAsset yarnFile)
    {
        StartDialogue(node, yarnFile);
        yield break;
    }


    private void StartDialogue(string node, TextAsset yarnFile)
    {
        AddDialogueDataIfNeeded(node, yarnFile);
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

    public DialogueAgentData GetDialogueData() => _dialogueAgentData;

    public bool CanSpeak() => !_dialogueRunner.isDialogueRunning;

    private void AddDialogueDataIfNeeded(string nodeName, TextAsset yarnFile)
    {
        if (yarnFile != null && !_dialogueRunner.NodeExists(nodeName)) // TODO throw error if no nodes are loaded, check why
        {
            Debug.Log("Added script on dialogue runner: " + yarnFile.name + " for node: " + nodeName);
            _dialogueRunner.AddScript(yarnFile);
        }
    }
}
