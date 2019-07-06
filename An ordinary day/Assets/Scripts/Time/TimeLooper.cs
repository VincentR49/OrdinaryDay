using UnityEngine;
using System;

public class TimeLooper : MonoBehaviour
{
    [SerializeField]
    private SceneSwitcher _loopSceneSwitch = default;
    [SerializeField]
    private SerialDateTime _startDate = default;
    [SerializeField]
    private SerialDateTime _endDate = default;
    [SerializeField]
    private DateTimeData _currentTime = default;
    [SerializeField]
    private bool _pauseOnTimeLoopStarted = true;

    private DateTime StartDateTime => _startDate.ToDateTime();
    private DateTime EndDateTime => _endDate.ToDateTime();
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
        ResetTime();
        _timeLoopStarting = false;
    }
}
