using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    [SerializeField]
    private WorldClock _worldClock;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (WorldClock.IsPaused)
                _worldClock.Resume();
            else
                _worldClock.Pause();
        }
    }
}
