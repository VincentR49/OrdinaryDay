using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Manager (future singleton), dealing with the dialogue with NPC
/// Probably not so usefull, check if it necessary or not
/// Will be removed later
/// </summary>
public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField]
    private DialogueRunner _dialogueRunner;
    [SerializeField]
    private DialogueWithNpcUIBehaviour _dialogueUI;
    [SerializeField]
    private NPCDataList _allNpcDataList;


    private void Awake()
    {
        // Load scripts
        foreach (var npc in _allNpcDataList.Items)
        {
            if (npc.YarnDialogue != null)
            {
                // TODO optimise, probably very heavy with a lot of text
                // upadte progressively as the players spawn
                // and keep track of each scripts that have been added here
                // Do when npc spawn and load at this moment (checking if the node exists or not)
                _dialogueRunner.AddScript(npc.YarnDialogue);
            }
        }
    }

    public bool DialogueIsRunning() => _dialogueRunner.isDialogueRunning;


    public void StartDialogue(NPCController npc)
    {
        Debug.Log("Start dialogue with " + npc.GetNPCData().FirstName);
        _dialogueRunner.StartDialogue(npc.GetNPCData().StartNodeStory);
    }
}
