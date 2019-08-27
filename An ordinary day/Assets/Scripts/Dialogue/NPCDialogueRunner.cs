using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Manager (future singleton), dealing with the dialogue with NPC
/// Probably not so usefull, check if it necessary or not
/// Will be removed later
/// </summary>
public class NPCDialogueRunner : DialogueRunner
{
    private void Awake()
    {
        NPCController.OnNPCAdded += OnNPCCreated;
        // make it persistant between scenes
    }


    private void OnDestroy()
    {
        NPCController.OnNPCAdded -= OnNPCCreated;
    }


    private void OnNPCCreated(NPCController npc)
    {
        var npcData = npc.GetNPCData();
        if (npcData.YarnDialogue != null
                
                && !NodeExists(npcData.StartNodeStory))
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
