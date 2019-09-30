using System.Collections;
using Yarn;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manage dialogue view for inter NPC dialogue
/// </summary>
public class InterNPCDialogueUIBehaviour : Yarn.Unity.DialogueUIBehaviour
{
    [SerializeField]
    private DialogueAgentDataList _allDialogueAgents; // as reference


    private List<NPCController> AllNPCs => NPCController.GetNPCControllers(); // all the instances of NPCs


    public override IEnumerator RunCommand(Command command)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator RunLine(Line line)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        throw new System.NotImplementedException();
    }
}
