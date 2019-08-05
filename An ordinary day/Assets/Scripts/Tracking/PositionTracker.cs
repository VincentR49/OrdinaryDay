using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Track and store the position of the game object in the linked PositionTrackingData scriptable object.
/// </summary>
public class PositionTracker : MonoBehaviour
{
    private PositionTrackingData _trackingData;
    private string _currentScenePath;
    private bool _isInit;

    public void Init(PositionTrackingData trackingData)
    {
        _trackingData = trackingData;
        _trackingData.LastPosition.Value.ScenePath = SceneManager.GetActiveScene().path;
        _isInit = true;
    }


    private void Update()
    {
        if (_isInit)
        {
            _trackingData.LastPosition.Value.Position = transform.position;
            _trackingData.LastPosition.TimeSec = Time.time;
        }
    }
}
