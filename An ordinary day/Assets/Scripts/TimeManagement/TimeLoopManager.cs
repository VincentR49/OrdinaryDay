using UnityEngine;
using System;

public class TimeLoopManager : MonoBehaviour
{
    [SerializeField]
    private SerialDateTime _startDate;
    [SerializeField]
    private SerialDateTime _endDate;
    [SerializeField]
    private WorldClock _worldClock;

    private DateTime _startDateTime;
    private DateTime _endDateTime;


    private void Awake()
    {
        _startDateTime = _startDate.ToDateTime();
        _endDateTime = _endDate.ToDateTime();
    }


    private void Start()
    {
        StartNewLoop();
    }


    private void Update()
    {
        if (WorldClock.GetTime() >= _endDateTime)
        {
            StartNewLoop();
        }
    }


    public void StartNewLoop()
    {
        Debug.Log("[TimeLoopManager] Start new loop");
        _worldClock.SetTime(_startDateTime);
    }
}
