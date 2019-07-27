using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Manage the schedule of all the pnj included in the pnjList
/// Initialise / Reset the schedule on each new time loop
/// Manage the spawn of pnj on change scene and simulate the pnj behaviours between each scenes change
/// 
/// TODO: probably will need to but in several small pieces
/// </summary>
public class PNJSchedulesManager : MonoBehaviour
{
    [SerializeField]
    private PNJDataList _pnjList;
    [SerializeField]
    private DateTimeData _currentTime;

    private static bool _alreadyExists;
    private List<Schedule> _pnjSchedules; // store all the pnj schedule references for conveniance


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
    }

    // Attach in inspector to a game event listener
    public void OnTimeLoopStarted()
    {
        InitPNJSchedules();
        // Make PNJ Spawn
    }


    private void InitPNJSchedules()
    {
        Debug.Log("[PNJSchedulesManager] Init PNJ schedules");
        _pnjSchedules = new List<Schedule>();
        foreach (var pnj in _pnjList.List)
            _pnjSchedules.Add (InitPNJSchedule(pnj));
    }


    private Schedule InitPNJSchedule(PNJData pnj)
    {
        Debug.Log("[PNJSchedulesManager] Init schedule of " + pnj.FirstName);
        var defaultSchedule = pnj.DefaultSchedule;
        var inGameSchedule = pnj.InGameSchedule;
        inGameSchedule.Copy(defaultSchedule);
        inGameSchedule.Reset();
        return inGameSchedule;
    }
}
