using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Behaviour that enable to speak with a nearby NPC
/// </summary>
public class NPCDialogueStarter : MonoBehaviour
{
    // TODO
    [SerializeField]
    private float _interactionRadius;

    private List<PNJController> InstanciateNPCs => PNJController.GetPNJControllers();
    private NPCDialogueManager _dialogueManager;

    private void Start()
    {
		_dialogueManager = FindObjectOfType<NPCDialogueManager>(); // change this later
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var npc = CheckForNearbyNPC();
            if (npc != null)
            {
                var pnjData = npc.GetPNJData();
                Debug.Log("Start dialogue with " + pnjData.FirstName);
				_dialogueManager.StartDialogueWith(npc);
            }
        }
    }


    private PNJController CheckForNearbyNPC()
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
