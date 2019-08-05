using UnityEngine;
using System;

// Manage the world time clock
// Singleton class
public class WorldClock : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Value of 1sec IRL in the game")]
    private FloatData _inGameTimeMultiplier;
    [SerializeField]
    private RuntimeDateTime _currentTime = default;
    public bool IsRunning => enabled;

    private void Awake()
    {
        Debug.Log("Start time: " + _currentTime.Value);
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
        UpdateTime(Time.deltaTime * _inGameTimeMultiplier.Value);
    }


    private void UpdateTime(float secondsSinceLastRefresh)
    {
        _currentTime.Value = _currentTime.Value.AddSeconds(secondsSinceLastRefresh);
    }


    public void Resume()
    {
        enabled = true;
    }


    public void Stop()
    {
        enabled = false;
    }
}
