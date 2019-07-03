﻿using UnityEngine;
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

    private DateTime StartDateTime => _startDate.ToDateTime();
    private DateTime EndDateTime => _endDate.ToDateTime();


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
        ResetTime();
    }


    public void StartNewLoop()
    {
        Debug.Log("[TimeLooper] Start new loop");
        ResetTime();
        _loopSceneSwitch.Switch();
    }


    private void ResetTime()
    {
        _currentTime.Value = StartDateTime;
    }
}
