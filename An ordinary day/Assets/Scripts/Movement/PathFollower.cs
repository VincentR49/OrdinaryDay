using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Allow to follow a path
/// </summary>
public class PathFollower : MonoBehaviour
{
    // define the minimal distance from the current target needed to go to the next target 
    private const float ReachedTargetMinDistance = 0.2f; 

    [SerializeField]
    private WalkManager _walkManager;

    private Queue<Vector2> _path;
    private Vector2 Target => _path.Peek();
    public bool IsFollowing { private set; get; }


    public void FollowPath(Queue<Vector2> path)
    {
        if (path == null || path.Count == 0)
            return;
        Debug.Log("Start following path: " + string.Join(",", path));
        IsFollowing = true;
        _path = path;
    }


    public void Stop(bool resetPath = false)
    {
        Debug.Log("Stop following path");
        IsFollowing = false;
        _walkManager.Stop();
        if (resetPath)
            _path = null;
    }


    private void Update()
    {
        if (IsFollowing)
        {
            if (HasReachedCurrentTarget())
            {
                if (!GoToNextStep())
                    return;
            }
            _walkManager.Move(Target - new Vector2(transform.position.x, transform.position.y));
        }
    }


    private bool HasReachedCurrentTarget()
    {
        if (_path == null || _path.Count == 0)
            return true;
        return Utils.Distance(transform.position, Target) <= ReachedTargetMinDistance;
    }


    private bool GoToNextStep()
    {
        _path.Dequeue();
        if (_path.Count == 0)
        {
            Stop();
            return false;
        }
        return true;
    }
}
