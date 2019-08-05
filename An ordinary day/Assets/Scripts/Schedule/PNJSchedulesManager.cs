using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manage the schedule of all the pnj included in the pnjList
/// Initialise / Reset the schedule on each new time loop
/// Manage the switch on / off of simulated schedules
/// </summary>
public class PNJSchedulesManager : MonoBehaviour
{
    [SerializeField]
    private PNJDataList _allPNJs;
    [SerializeField]
    private PNJControllerList _inGamePNJs;
    [SerializeField]
    private RuntimeDateTime _currentTime;
    [SerializeField]
    private GameObject _simulatedScheduleHandlerPrefab;

    private static bool _alreadyExists;
    private Dictionary<PNJData, ScheduleHandler> _simulatedScheduleHandlerDict;


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
        _inGamePNJs.OnItemAdded += OnPNJCreated;
        _inGamePNJs.OnItemRemoved += OnPNJDestroyed;
        InitSchedules(); 
    }


    private void OnDestroy()
    {
        _inGamePNJs.OnItemAdded -= OnPNJCreated;
        _inGamePNJs.OnItemRemoved -= OnPNJDestroyed;
    }

    #region Init

    private void InitSchedules()
    {
        Debug.Log("[PNJSchedulesManager] InitSimulatedScheduleHandlers");
        _simulatedScheduleHandlerDict = new Dictionary<PNJData, ScheduleHandler>();
        foreach (var pnj in _allPNJs.Items)
        {
            // init the runtime schedule of the pnj
            pnj.InitRuntimeSchedule();
            // Create one simulated schedule handler for each pnjs
            var go = Instantiate(_simulatedScheduleHandlerPrefab, transform);
            var scheduleHandler = go.GetComponent<ScheduleHandler>();
            scheduleHandler.Init(pnj.InGameSchedule);
            _simulatedScheduleHandlerDict.Add(pnj, scheduleHandler);
            // Update the status of the simulated schedule
            RefreshScheduleHandlersStatus(scheduleHandler, pnj);
        }
    }


    private void RefreshScheduleHandlersStatus(ScheduleHandler scheduleHandler, PNJData pnjData)
    {
        var pnjExists = _inGamePNJs.Items == null ? false : _inGamePNJs.Items.Find(x => x.GetPNJData() == pnjData) != null;
        EnableSimulatedScheduleHandler(scheduleHandler, !pnjExists); 
    }


    private void EnableSimulatedScheduleHandler(ScheduleHandler scheduleHandler, bool enable)
    {
        scheduleHandler.enabled = enable;
    }

    #endregion


    private void ResetPNJSchedules()
    {
        foreach (var entry in _simulatedScheduleHandlerDict)
            entry.Value.GetSchedule().Reset();
    }

    
    #region Event responses
    // Attach in inspector to a game event listener
    public void OnTimeLoopStarted()
    {
        ResetPNJSchedules();
    }


    public void OnPNJCreated(PNJController pnj)
    {
        Debug.Log("OnPNJCreated: " + pnj.GetPNJData());
        var scheduleHandler = _simulatedScheduleHandlerDict[pnj.GetPNJData()];
        EnableSimulatedScheduleHandler(scheduleHandler, false);
    }


    public void OnPNJDestroyed(PNJController pnj)
    {
        Debug.Log("OnPNJDestroyed: " + pnj.GetPNJData());
        var scheduleHandler = _simulatedScheduleHandlerDict[pnj.GetPNJData()];
        EnableSimulatedScheduleHandler(scheduleHandler, true);
    }
    #endregion
}
