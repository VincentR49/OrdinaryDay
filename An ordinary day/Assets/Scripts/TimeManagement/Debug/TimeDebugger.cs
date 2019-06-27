using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    [SerializeField]
    private WorldClock _worldClock = default;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_worldClock.IsRunning)
                _worldClock.Stop();
            else
                _worldClock.Resume();
        }
    }
}
