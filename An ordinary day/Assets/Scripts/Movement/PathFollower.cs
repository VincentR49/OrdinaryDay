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
    private int _initPathLength;
    private int _step;
    public float ProgressPrc => IsRunning ? (float) _step / _initPathLength : 0f;
    public Vector2 Target => _path.Peek();
    public bool IsRunning { private set; get; }
    private float _previousSpeed;
    // define the minimal distance from the current target needed to go to the next target 
    private float ReachedTargetMinDistance => _walkManager.Speed * Time.deltaTime;
    public delegate void FinalTargetReachedHandler();

    public event FinalTargetReachedHandler OnFinalTargetReached; // fired when the final target of the path is reached

    public void FollowPath(Queue<Vector2> path)
    {
        if (path == null || path.Count == 0)
            return;  
        Debug.Log("Start following path.");
        IsRunning = true;
        _path = path;
        _initPathLength = _path.Count;
        _step = 0;
    }


    public void Stop()
    {
        Debug.Log("Stop following path");
        IsRunning = false;
        _walkManager.Stop();
    }


    public void Resume()
    {
        Debug.Log("Resume path following");
        IsRunning = true;
    }

    private void Update()
    {
        if (IsRunning)
        {
            var distance = Utils.Distance(transform.position, Target);
            var direction = Target - transform.position.ToVector2();
            // if we are less than one frame from the target
            if (distance < _walkManager.Speed * Time.deltaTime)
            {
                GoToNextStep();
                if (_path.Count != 0) // We move just what is needed to reach the target
                    _walkManager.Move(direction, distance / Time.deltaTime);
            }
            else
                _walkManager.Move(direction);
        }
    }


    private void GoToNextStep()
    {
        _path.Dequeue();
        _step++;
        if (_path.Count == 0)
        {
            Stop();
            Debug.Log("[PathFollower] Final target reached.");
            OnFinalTargetReached.Invoke();
        }
    }


    public void EditSpeed(float mutliplier)
    {
        _previousSpeed = _walkManager.GetDefaultSpeed();
        _walkManager.SetDefaultSpeed(_previousSpeed * mutliplier);
    }


    public void ResetSpeed()
    {
        _walkManager.SetDefaultSpeed(_previousSpeed);
    }
}
