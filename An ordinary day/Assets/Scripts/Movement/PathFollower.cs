using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Allow to follow a path
/// </summary>
public class PathFollower : MonoBehaviour
{
   
    [SerializeField]
    private WalkManager _walkManager;

    private Queue<Vector2> _path;
    private Vector2 Target => _path.Peek();
    public bool IsFollowing { private set; get; }

    // define the minimal distance from the current target needed to go to the next target 
    private float ReachedTargetMinDistance => _walkManager.Speed * Time.deltaTime;
    public delegate void FinalTargetReachedHandler();

    public event FinalTargetReachedHandler OnFinalTargetReached; // fired when the final target of the path is reached

    public void FollowPath(Queue<Vector2> path)
    {
        if (path == null || path.Count == 0)
            return;
        //Debug.Log("Start following path: " + string.Join(",", path));
        Debug.Log("Start following path.");
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
            var distance = Utils.Distance(transform.position, Target);
            var direction = new Vector2(Target.x - transform.position.x, Target.y - transform.position.y);
            if (distance < _walkManager.Speed * Time.deltaTime)
            {
                GoToNextStep();
                if (_path.Count != 0)
                    _walkManager.Move(direction, distance / Time.deltaTime);
                
            }
            else
                _walkManager.Move(direction);
        }
    }


    private bool HasReachedCurrentTarget()
    {
        if (_path == null || _path.Count == 0)
            return true;
        return Utils.Distance(transform.position, Target) < ReachedTargetMinDistance;
    }


    private void GoToNextStep()
    {
        _path.Dequeue();
        if (_path.Count == 0)
        {
            Stop();
            Debug.Log("[PathFollower] Final target reached.");
            OnFinalTargetReached.Invoke();
        }
    }
}
