using UnityEngine;
using System;

// Manage the world time clock
// Singleton class
public class WorldClock : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Value of 1sec IRL in the game")]
    private float _inGameTimeMultiplier = 60f;
    [SerializeField]
    private DateTimeData _currentTime;

    private static WorldClock _instance;
    public bool IsRunning { private set; get; }
    public static DateTime Date => _instance._currentTime.Value;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Resume();
    }


    public void SetTime(DateTime dateTime)
    {
        Debug.Log("Init World Clock");
        _currentTime.Value = dateTime;
    }


    private void Update()
    {
        if (!IsRunning) 
            return;
        UpdateTime(Time.deltaTime * _inGameTimeMultiplier);
    }


    private void UpdateTime(float secondsSinceLastRefresh)
    {
        _currentTime.Value = _currentTime.Value.AddSeconds(secondsSinceLastRefresh);
    }


    public void Resume()
    {
        IsRunning = true;
    }


    public void Stop()
    {
        IsRunning = false;
    }
}
