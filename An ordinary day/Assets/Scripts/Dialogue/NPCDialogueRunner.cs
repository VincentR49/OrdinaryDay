using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Specific DialogueRunner class for NPC dialogues
/// Ineritate from DialogueRunner of Yarn.
/// </summary>
public class NPCDialogueRunner : DialogueRunner
{
    private void Awake()
    {
        NPCController.OnNPCAdded += OnNPCCreated;
        // make it persistant between scenes ?
    }


    private void OnDestroy()
    {
        NPCController.OnNPCAdded -= OnNPCCreated;
    }


    private void OnNPCCreated(NPCController npc)
    {
        var npcData = npc.GetNPCData();
        // We add script progresively, as NPC spawns
        if (npcData.YarnDialogue != null && !NodeExists(npcData.StartNodeStory))
        {
            Debug.Log("Added script on dialogue runner: " + npcData.StartNodeStory);
            AddScript(npcData.YarnDialogue);
        }
    }


    public void StartDialogue(NPCController npc)
    {
        Debug.Log("Start dialogue with " + npc.GetNPCData().FirstName);
        StartDialogue(npc.GetNPCData().StartNodeStory);
    }
}
