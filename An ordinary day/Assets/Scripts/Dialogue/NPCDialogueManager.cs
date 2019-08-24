using UnityEngine;
using Yarn.Unity;

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
        foreach (var npc in _allNpcDataList.Items)
        {
            if (npc.YarnDialogue != null)
            {
                // TODO optiise, probably very heavy with a lot of text
                // upadte progressively as the players spawn
                // and keep track of each scripts that have been added here
                // Do when npc spawn and load at this moment (checking if the node exists or not)
                _dialogueRunner.AddScript(npc.YarnDialogue);
            }
        }
    }



    public void StartDialogueWith(NPCController npc)
    {
        _dialogueUI.SetCurrentNPC(npc);
        _dialogueRunner.StartDialogue(npc.GetNPCData().StartNodeStory);
    }
}
