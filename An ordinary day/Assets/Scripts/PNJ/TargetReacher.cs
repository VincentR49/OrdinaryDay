using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Enable a PNJ to find and follow a path to the given target
/// Associate a pathfinder behaviour and a path follower one
/// </summary>
public class TargetReacher : MonoBehaviour
{
    // Constants to balance
    private const float FollowingPathScanRefreshRate = 5f;
    private const float ScanRadius = 1.5f; // in unity unit
    private const int MaxTrials = 10;
    private const float WaitBetweenTrialSec = 3f;

    [SerializeField]
    private PathFinder _pathFinder;
    [SerializeField]
    private PathFollower _pathFollower;

    private Vector2 _target;
    private bool _currentDestinationIsTarget;
    private ColliderScanner _colliderScanner;
    private Collider2D _pnjCollider;
    private float _lastSearchTargetTime;
    private int _trialNumber;

    #region Init
    private void Awake()
    {
        _pnjCollider = GetComponent<Collider2D>();
    }


    private void Start()
    {
        InitPathFindingSystem();
    }


    private void InitPathFindingSystem()
    {
        _colliderScanner = FindObjectOfType<ColliderScanner>();
        if (_colliderScanner == null)
        {
            Debug.LogError("Collider scanner not found !");
            return;
        }
        _pathFollower.OnFinalTargetReached += OnCurrentTargetReached;
        _pathFinder.SetColliderScanner(_colliderScanner);
    }
    #endregion


    private void OnDestroy()
    {
        _pathFollower.OnFinalTargetReached -= OnCurrentTargetReached;
    }

    #region PathFinding

    public void GoToTarget(Vector2 target)
    {
        _target = target;
        var path = _pathFinder.FindShortestPath(transform.position, target, out _currentDestinationIsTarget, new Collider2D[] { _pnjCollider });
        _pathFollower.FollowPath(new Queue<Vector2>(path));
        _lastSearchTargetTime = Time.time;
    }


    private void OnCurrentTargetReached()
    {
        Debug.Log("[PNJTargetReacher] OnCurrentTargetReached");
        if (!_currentDestinationIsTarget)
        {
            if (_trialNumber > MaxTrials)
            {
                Debug.LogError("Couldnt reach target after " + MaxTrials + ". Behaviour to implement here...");
                _trialNumber = 0;
            }
            else
            {
                StartCoroutine(TryNewAttemptToGoToTarget());
            }
        }
        else
        {
            _trialNumber = 0;
        }
    }


    private IEnumerator TryNewAttemptToGoToTarget()
    {
        yield return new WaitForSeconds(WaitBetweenTrialSec);
        _trialNumber++;
        Debug.Log("Start new attempt to go to the target: " + _trialNumber + " trial");
        RefreshPath();
    }


    // todo check if it really optimized ? check for collision instead? weight big or small collisions?
    private void Update()
    {
        /*
        if (!_pathFollower.IsFollowing)
            return;
        if (Time.time > (_lastSearchTargetTime + FollowingPathScanRefreshRate))
        {
            RefreshPath();
        }
        */
    }


    private void RefreshPath()
    {
        ScanNearbyArea();
        GoToTarget(_target);
    }

    private void ScanNearbyArea()
    {
        _colliderScanner.Scan(transform.position, ScanRadius);
    }
    #endregion
}
