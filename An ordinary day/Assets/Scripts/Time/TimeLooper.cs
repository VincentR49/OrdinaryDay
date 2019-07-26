using UnityEngine;
using System;

public class TimeLooper : MonoBehaviour
{
    private static DateTime NoTime = new DateTime();
    [SerializeField]
    private WorldClock _worldClock;
    [SerializeField]
    private SceneSwitcher _loopSceneSwitch = default;
    [SerializeField]
    private SerialDateTimeData _startDate = default;
    [SerializeField]
    private SerialDateTimeData _endDate = default;
    [SerializeField]
    private DateTimeData _currentTime = default;
    [SerializeField]
    private bool _pauseOnTimeLoopStarted = true;

    private DateTime StartDateTime => _startDate.Value.ToDateTime();
    private DateTime EndDateTime => _endDate.Value.ToDateTime();
    private bool _timeLoopStarting;

    private void Update()
    {
        if (_currentTime.Value >= EndDateTime)
        {
            StartNewLoop();
        }
    }

    // To Call to init the timeLooper
    public void Init()
    {
        Debug.Log("Init Time Looper");
        OnTimeLoopStart();
    }


    public void StartNewLoop()
    {
        if (_timeLoopStarting)
            return;
        Debug.Log("[TimeLooper] Start new loop");
        _worldClock.Stop();
        _currentTime.Value = NoTime;
        _timeLoopStarting = true;
        _loopSceneSwitch.Switch(OnTimeLoopStart);
    }


    private void ResetTime()
    {
        _currentTime.Value = StartDateTime;
    }


    private void OnTimeLoopStart()
    {
        if (_pauseOnTimeLoopStarted)
            GamePauser.Pause();
        _timeLoopStarting = false;
        ResetTime();
        _worldClock.Resume();
    }
}
