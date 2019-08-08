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
        _isInit = true;
    }


    public void UpdatePosition(Vector2 position)
    {
        UpdatePosition(new GamePosition(SceneManager.GetActiveScene().path, position));
    }


    public void UpdatePosition(GamePosition gamePosition)
    {
        if (!_isInit)
        {
            Debug.LogError("Position tracker not initialized.");
            return;
        }
        _trackingData.GamePosition = gamePosition;
    }
}
