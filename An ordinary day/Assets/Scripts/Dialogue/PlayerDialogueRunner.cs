﻿using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Specific DialogueRunner class for Player based dialogue
/// Ineritate from DialogueRunner of Yarn.
/// </summary>
public class PlayerDialogueRunner : DialogueRunner
{
    private void Awake()
    {
        NPCController.OnNPCAdded += OnNPCCreated;
        InteractibleObject.OnInteractibleObjectCreated += OnInteractibleObjectCreated;
        // make it persistant between scenes ?
    }


    private void OnDestroy()
    {
        NPCController.OnNPCAdded -= OnNPCCreated;
        InteractibleObject.OnInteractibleObjectCreated -= OnInteractibleObjectCreated;
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


    private void OnInteractibleObjectCreated(InteractibleObjectData objectData)
    {
        if (objectData.YarnDialogue != null && !NodeExists(objectData.DefaultNodeStory))
        {
            Debug.Log("Added script on dialogue runner: " + objectData.DefaultNodeStory);
            AddScript(objectData.YarnDialogue);
        }
    }
}
