using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour that enable to speak with a nearby NPC
/// </summary>
public class SpeakToNPCStarter : MonoBehaviour
{
    [SerializeField]
    private float _interactionRadius;

    private List<NPCController> InstanciateNPCs => NPCController.GetNPCControllers();
    private NPCDialogueManager _dialogueManager;
    private WalkManager _walkManager;
    
    private void Start()
    {
		_dialogueManager = FindObjectOfType<NPCDialogueManager>(); // change this later
        _walkManager = GetComponent<WalkManager>();
    }


    private void Update()
    {
        if (_dialogueManager != null && _dialogueManager.DialogueIsRunning())
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var npc = CheckForNearbyNPC();
            if (npc != null)
            {
				_dialogueManager.StartDialogue(npc);
                npc.OnDialogueStarted();
                if (_walkManager)
                    _walkManager.Stop();
            }
        }
    }


    private NPCController CheckForNearbyNPC()
    {
        Debug.Log("CheckForNearbyNPC");
        if (InstanciateNPCs == null)
            return null;
        foreach (var npcController in InstanciateNPCs)
        {
            if (Utils.Distance(transform.position, npcController.transform.position) < _interactionRadius)
                return npcController;
        }
        return null;
    }
}
