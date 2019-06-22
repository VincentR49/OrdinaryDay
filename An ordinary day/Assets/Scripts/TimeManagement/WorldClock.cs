﻿using UnityEngine;
using System;

// Manage the world time clock
// Singleton class
public class WorldClock : MonoBehaviour
{
    [SerializeField]
    private float _gameTimeScale = 1f;
    [SerializeField]
    [Tooltip("Value of 1sec IRL in the game")]
    private float _inGameTimeMultiplier = 60f;

    private static WorldClock _instance;
    private DateTime _currentTime;
    public static bool IsPaused => Math.Abs(Time.timeScale) < 0.001f;


    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Cannot instanciate more than one World Clock!");
            Destroy(this);
            return;
        }
        Init();
    }


    private void Init()
    {
        _instance = this;
        _currentTime = DateTime.Now;
        Time.timeScale = _gameTimeScale;
        DontDestroyOnLoad(this); // persistant time
    }


    public void SetTime(DateTime time)
    {
        _currentTime = time;
    }
   

    private void Update()
    {
        RefreshTime(Time.deltaTime * _inGameTimeMultiplier);
    }


    private void RefreshTime(float secondsSinceLastRefresh)
    {
        _currentTime = _currentTime.AddSeconds(secondsSinceLastRefresh);
    }


    public void Pause()
    {
        Time.timeScale = 0;
    }


    public void Resume()
    {
        Time.timeScale = _gameTimeScale;
    }


    public static DateTime GetTime() => _instance._currentTime;
}
