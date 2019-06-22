using UnityEngine;
using UnityEngine.Events;
using System;

public class TimeLooper : MonoBehaviour
{
    public UnityEvent NewLoopStarted;

    [SerializeField]
    private SerialDateTime _startDate;
    [SerializeField]
    private SerialDateTime _endDate;
    [SerializeField]
    private WorldClock _worldClock;

    private DateTime _startDateTime;
    private DateTime _endDateTime;
    private bool _isRunning;


    private void Awake()
    {
        _startDateTime = _startDate.ToDateTime();
        _endDateTime = _endDate.ToDateTime();
        _isRunning = false;
    }


    private void Update()
    {
        if (!_isRunning) return;

        if (WorldClock.GetTime() >= _endDateTime)
        {
            StartNewLoop();
        }
    }

    // To Call to init the timeLooper
    public void Init()
    {
        _isRunning = true;
        StartNewLoop(false);
    }


    public void StartNewLoop(bool notify = true)
    {
        Debug.Log("[TimeLooper] Start new loop");
        _worldClock.SetTime(_startDateTime);
        if (notify)
            NewLoopStarted.Invoke();
    }
}
