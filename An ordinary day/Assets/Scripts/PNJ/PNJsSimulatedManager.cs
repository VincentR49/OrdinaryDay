using UnityEngine;

/// <summary>
/// Manage the schedule of all the pnj included in the pnjList
/// Initialise / Reset the schedule on each new time loop
/// Manage the switch on / off of simulated schedules
/// </summary>
public class PNJsSimulatedManager : MonoBehaviour
{
    [SerializeField]
    private PNJDataList _allPNJs;
    [SerializeField]
    private PNJSimulatedController _PNJSimulatedPrefab;

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
        InitSimulatedPNJs(); 
    }

    private void InitSimulatedPNJs()
    {
        Debug.Log("[PNJSchedulesManager] InitSimulatedPNJs");
        foreach (var pnj in _allPNJs.Items)
        {
            // Create one simulated schedule handler for each pnjs
            var go = Instantiate(_PNJSimulatedPrefab, transform);
            go.Init(pnj);
        }
    }
}
