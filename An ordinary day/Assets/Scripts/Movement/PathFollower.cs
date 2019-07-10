using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Allow to follow a path
/// </summary>
public class PathFollower : MonoBehaviour
{
    // define the minimal distance from the current target needed to go to the next target 
    private const float ReachedTargetMinDistance = 0.5f; 

    [SerializeField]
    private WalkManager _walkManager;

    private bool _isFollowingPath;
    private List<Vector2> _path;
    private Vector2 _target;


    public void FollowPath(List<Vector2> path)
    {
        Debug.Log("Start following path");
        _isFollowingPath = true;
        _path = path;
    }


    public void Stop()
    {
        if (!_isFollowingPath) return;
        _isFollowingPath = false;
        _walkManager.Stop();
    }


    private void Update()
    {
        if (_isFollowingPath)
        {
            // todo do something
        }
    }


    private Vector2 GetNextTarget()
    {
        // todo
        return Vector2.zero;
    }
}
