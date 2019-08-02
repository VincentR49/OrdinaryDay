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
    private DateTimeData _currentTime;
    [SerializeField]
    private GameObject _simulatedScheduleHandlerPrefab;

    private static bool _alreadyExists;
    private List<Schedule> _pnjSchedules; // store all the pnj schedule references for conveniance
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
        InitSimulatedScheduleHandlers();
        RefreshScheduleHandlersStatus();
    }


    private void OnDestroy()
    {
        _inGamePNJs.OnItemAdded -= OnPNJCreated;
        _inGamePNJs.OnItemRemoved -= OnPNJDestroyed;
    }

    #region Init

    private void InitSimulatedScheduleHandlers()
    {
        Debug.Log("[PNJSchedulesManager] InitSimulatedScheduleHandlers");
        _simulatedScheduleHandlerDict = new Dictionary<PNJData, ScheduleHandler>();
        foreach (var pnj in _allPNJs.Items)
        {
            var go = Instantiate(_simulatedScheduleHandlerPrefab, transform);
            var scheduleHandler = go.GetComponent<ScheduleHandler>();
            scheduleHandler.Init(pnj.InGameSchedule);
            _simulatedScheduleHandlerDict.Add(pnj, scheduleHandler);
        }
    }


    private void RefreshScheduleHandlersStatus()
    {
        Debug.Log("[PNJSchedulesManager] InitSimulatedScheduleHandlersStatus");
        foreach (var entry in _simulatedScheduleHandlerDict)
        {
            var pnjData = entry.Key;
            var controller = _inGamePNJs.Items.Find(x => x.GetPNJData() == pnjData);
            EnableSimulatedScheduleHandler(entry.Value, controller == null);
        }
    }


    private void EnableSimulatedScheduleHandler(PNJData pnjData, bool enable)
    {
        EnableSimulatedScheduleHandler(_simulatedScheduleHandlerDict[pnjData], enable);
    }

    private void EnableSimulatedScheduleHandler(ScheduleHandler scheduleHandler, bool enable)
    {
        scheduleHandler.enabled = enable;
    }

    #endregion


    #region Schedule Init

    private void InitPNJSchedules()
    {
        Debug.Log("[PNJSchedulesManager] Init PNJ schedules");
        _pnjSchedules = new List<Schedule>();
        foreach (var pnj in _allPNJs.Items)
            _pnjSchedules.Add(InitPNJSchedule(pnj));
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

    #endregion


    #region Event responses
    // Attach in inspector to a game event listener
    public void OnTimeLoopStarted()
    {
        InitPNJSchedules();
    }


    public void OnPNJCreated(PNJController pnj)
    {
        Debug.Log("OnPNJCreated: " + pnj.GetPNJData());
        EnableSimulatedScheduleHandler(pnj.GetPNJData(), false);
    }


    public void OnPNJDestroyed(PNJController pnj)
    {
        Debug.Log("OnPNJDestroyed: " + pnj.GetPNJData());
        EnableSimulatedScheduleHandler(pnj.GetPNJData(), true);
    }
    #endregion
}
