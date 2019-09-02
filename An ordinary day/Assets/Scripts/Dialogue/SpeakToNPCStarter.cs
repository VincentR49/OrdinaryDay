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
    private NPCDialogueRunner _npcdialogueRunner;
    private WalkManager _walkManager;
    private SpriteDirectioner _spriteDirectioner;


    private void Start()
    {
		_npcdialogueRunner = FindObjectOfType<NPCDialogueRunner>(); // change this later
        _walkManager = GetComponent<WalkManager>();
        _spriteDirectioner = GetComponent<SpriteDirectioner>();
    }


    public bool StartDialogueIfPossible()
    {
        if (_npcdialogueRunner.isDialogueRunning)
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
        _npcdialogueRunner.StartDialogue(npc);
    }


    public NPCController CheckForNearbyNPC()
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
