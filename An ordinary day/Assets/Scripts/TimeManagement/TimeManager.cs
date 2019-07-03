using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    [SerializeField]
    private WorldClock _worldClock;
    [SerializeField]
    private TimeLooper _timeLooper;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Several instances of TimeManager are detected.");
            DestroyImmediate(gameObject);
        }
    }


    public static void StopTime()
    {
        _instance._worldClock.Stop();
    }


    public static void ResumeTime()
    {
        _instance._worldClock.Resume();
    }


    public static void Init()
    {
        _instance._timeLooper.Init();
        ResumeTime();
    }
}
