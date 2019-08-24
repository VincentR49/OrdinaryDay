using UnityEngine;

/// <summary>
/// Manage the schedule of all the npc included in the npcList
/// Initialise / Reset the schedule on each new time loop
/// Manage the switch on / off of simulated schedules
/// </summary>
public class NPCsSimulatedManager : MonoBehaviour
{
    [SerializeField]
    private NPCDataList _allNPCs;
    [SerializeField]
    private NPCSimulatedController _npcSimulatedPrefab;

    private static bool _alreadyExists;

    private void Awake()
    {
        if (!_alreadyExists)
        {
            _alreadyExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        InitSimulatedNPCs(); 
    }

    private void InitSimulatedNPCs()
    {
        Debug.Log("[NPCSchedulesManager] InitSimulatedNPCs");
        foreach (var npc in _allNPCs.Items)
        {
            // Create one simulated schedule handler for each npcs
            var go = Instantiate(_npcSimulatedPrefab, transform);
            go.Init(npc);
        }
    }
}
