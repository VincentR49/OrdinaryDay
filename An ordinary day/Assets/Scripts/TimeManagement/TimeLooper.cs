using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class TimeLooper : MonoBehaviour
{
    [SerializeField]
    private string _firstSceneTag = "MainScene";

    [SerializeField]
    private SerialDateTime _startDate;
    [SerializeField]
    private SerialDateTime _endDate;
    [SerializeField]
    private DateTimeData _currentTime;

    private static TimeLooper _instance;
    private DateTime StartDateTime => _startDate.ToDateTime();
    private DateTime EndDateTime => _endDate.ToDateTime();


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
        Init();
    }


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
        SceneManager.LoadScene(_firstSceneTag);
    }


    private void ResetTime()
    {
        _currentTime.Value = StartDateTime;
    }
}
