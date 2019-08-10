using UnityEngine;
using System;

public class TimeLooper : MonoBehaviour
{
    private static DateTime NoTime = new DateTime();
    [SerializeField]
    private WorldClock _worldClock;
    [SerializeField]
    [Tooltip("Started the process of reseting time loop")]
    private GameEvent _onTimeLoopInitializedEvent;
    [SerializeField]
    [Tooltip("New loop completely started")]
    private GameEvent _onTimeLoopStartedEvent;
    [SerializeField]
    private SceneSwitcher _loopSceneSwitch = default;
    [SerializeField]
    private SerialDateTimeData _startDate = default;
    [SerializeField]
    private SerialDateTimeData _endDate = default;
    [SerializeField]
    private RuntimeDateTime _currentTime = default;
    [SerializeField]
    private bool _pauseOnTimeLoopStarted = true;

    private DateTime StartDateTime => _startDate.Value.ToDateTime();
    private DateTime EndDateTime => _endDate.Value.ToDateTime();
    private bool _timeLoopStarting;

    private void Update()
    {
        if (_currentTime.Value >= EndDateTime)
        {
            InitNewLoop();
        }
    }

    // To Call to init the timeLooper
    public void Init()
    {
        Debug.Log("Init Time Looper");
        OnTimeLoopStarted();
    }


    public void InitNewLoop()
    {
        if (_timeLoopStarting)
            return;
        Debug.Log("[TimeLooper] Start new loop");
        _worldClock.Stop();
        _currentTime.Value = NoTime;
        _timeLoopStarting = true;
        _onTimeLoopInitializedEvent.Raise();
        _loopSceneSwitch.Switch(OnTimeLoopStarted);
    }


    private void ResetTime()
    {
        _currentTime.Value = StartDateTime;
    }


    private void OnTimeLoopStarted()
    {
        if (_pauseOnTimeLoopStarted)
            GamePauser.Pause();
        _timeLoopStarting = false;
        ResetTime();
        _worldClock.Resume();
        _onTimeLoopStartedEvent.Raise();
    }
}
