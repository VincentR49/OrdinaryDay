using UnityEngine;

/// <summary>
/// Random walk script (debug)
/// </summary>
public class RandomWalker : MonoBehaviour
{
    [SerializeField]
    private WalkManager _walkManager;

    private const float SwitchDirectionRate = 1f;
    private float _lastChangeDirectionTime = 0f;
    private Vector2 _currentDirection = Vector2.zero;

    private void Update()
    {
        if (Time.time > _lastChangeDirectionTime + SwitchDirectionRate)
        {
            _currentDirection = GetRandomDirection();

            _lastChangeDirectionTime = Time.time;
        }
        _walkManager.Move(_currentDirection);
    }


    private static Vector2 GetRandomDirection()
    {
        return new Vector2(Mathf.Round(Random.Range(-1f, 1f)), Mathf.Round(Random.Range(-1f, 1f)));
    }
}
