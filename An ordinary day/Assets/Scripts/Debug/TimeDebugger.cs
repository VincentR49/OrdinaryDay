using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    [SerializeField]
    private WorldClock _worldClock;
    [SerializeField]
    private TimeLooper _timeLooper;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // stop time
        {
            if (_worldClock.IsRunning)
                _worldClock.Stop();
            else
                _worldClock.Resume();
        }

        if (Input.GetKeyDown(KeyCode.R)) // restart loop
        {
            _timeLooper.InitNewLoop();
        }
    }
}
