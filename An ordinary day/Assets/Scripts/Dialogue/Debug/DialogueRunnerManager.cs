using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug behaviour to try some stuff in yarn
/// </summary>
public class DialogueRunnerManager : MonoBehaviour
{
    [SerializeField]
    private Yarn.Unity.DialogueRunner _dialogueRunner;

    public Dictionary<string, int> _visitedNodes;

    private void Start()
    {
        _visitedNodes = _dialogueRunner.dialogue.visitedNodeCount;
    }


    public void UnloadDialogue()
    {
        _dialogueRunner.dialogue.UnloadAll();
    }
}
