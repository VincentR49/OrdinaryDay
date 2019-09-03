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
    private PlayerDialogueRunner _dialogueRunner;
    private WalkManager _walkManager;
    private SpriteDirectioner _spriteDirectioner;


    private void Start()
    {
		_dialogueRunner = FindObjectOfType<PlayerDialogueRunner>(); // change this later
        _walkManager = GetComponent<WalkManager>();
        _spriteDirectioner = GetComponent<SpriteDirectioner>();
    }


    public bool StartDialogueIfPossible()
    {
        if (_dialogueRunner.isDialogueRunning)
            return false;
        var npc = CheckForNearbyNPC();
        if (npc == null)
            return false;
        StartDialogue(npc);
        return true;
    }


    public void StartDialogue(NPCController npc)
    {
        npc.OnDialogueStarted(transform);
        if (_walkManager)
            _walkManager.Stop();
        if (_spriteDirectioner)
            _spriteDirectioner.FaceTowards(npc.transform);
        _dialogueRunner.StartDialogue(npc.GetNPCData().StartNodeStory);
    }


    public NPCController CheckForNearbyNPC()
    {
        Debug.Log("CheckForNearbyNPC");
        if (InstanciateNPCs == null)
            return null;
        // Better to check on the colliders around maybe??
        foreach (var npcController in InstanciateNPCs)
        {
            if (Utils.Distance(transform.position, npcController.transform.position) < _interactionRadius)
                return npcController;
        }
        return null;
    }
}
