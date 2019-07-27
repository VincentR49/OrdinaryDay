using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Enable a PNJ to find and follow a path to the given target
/// Associate a pathfinder behaviour and a path follower one
/// Enable also to avoid target in real time => TODO
/// </summary>
public class TargetReacher : MonoBehaviour
{
    // Constants to balance
    private const float FollowingPathScanRefreshRate = 1f;
    private const float ScanRadius = 2f; // in unity unit
    private const int MaxTrials = 3;
    private const float WaitBetweenTrialSec = 3f;
    
    // Avoiding system constants
    private const float AvoidingFirstWaitingTime = 3f;
    private const float AngryModeSpeedMultiplier = 2f;
    private const float AngryModeDuration = 1f;
    // TODO Idea to improve system
    // Add some probability to get angry / wait / avoid obstacle

    // Inspector
    [SerializeField]
    private PathFinder _pathFinder;
    [SerializeField]
    private PathFollower _pathFollower;

    // Events
    public delegate void OnTargetReachedHandler(Vector2 target);
    public event OnTargetReachedHandler OnTargetReached;

    // Data
    private Vector2 _finalTarget;
    private List<Vector2> _staticPath;
    private bool _currentDestinationIsTarget;
    private int _trialNumber;

    // Other util behaviours
    private ColliderScanner _colliderScanner;
    private Collider2D _myCollider;

    private Coroutine _avoidingRoutine;
    private bool _isAvoiding;
    private bool _isAngry;
    private Collider2D _colliderToAvoid;

    #region Init
    private void Awake()
    {
        _myCollider = GetComponent<Collider2D>();
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
        // search first only static path
        _finalTarget = target;
        _staticPath = _pathFinder.FindShortestPath(transform.position, target, out _currentDestinationIsTarget, true, new Collider2D[] { _myCollider });
        _pathFollower.FollowPath(new Queue<Vector2>(_staticPath));
    }


    public void Stop()
    {
        Debug.Log("Stop going to the target");
        _pathFollower.Stop();
    }

    private void OnCurrentTargetReached()
    {
        Debug.Log("[PNJTargetReacher] OnCurrentTargetReached");
        if (!_currentDestinationIsTarget)
        {
            if (_trialNumber > MaxTrials)
            {
                Debug.LogError("Couldnt reach target after " + MaxTrials + " trials. Behaviour to implement here...");
                _trialNumber = 0;
            }
            else
            {
                StartCoroutine(TryNewAttemptToGoToTarget());
            }
        }
        else // we reached the good target
        {
            _trialNumber = 0;
            OnTargetReached?.Invoke(_finalTarget);
        }    
    }

   
    private IEnumerator TryNewAttemptToGoToTarget()
    {
        yield return new WaitForSeconds(WaitBetweenTrialSec);
        _trialNumber++;
        Debug.Log("Start new attempt to go to the target: " + _trialNumber + " trial");
        ScanNearbyArea();
        GoToTarget(_finalTarget);
    }


    private void ScanNearbyArea()
    {
        _colliderScanner.Scan(transform.position, ScanRadius);
    }
    #endregion



    #region Avoid Obstacle

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // already avoiding or not following path, we skip
        if (!_pathFollower.IsRunning || _isAvoiding)
            return;
        // if the collider is not blocking the way to the target, we pass
        if (!AreSameDirection(collision.transform.position - transform.position,
                             _pathFollower.Target - transform.position.ToVector2()))
        {
            //Debug.Log("Collider is not blocking the way to the target, no avoiding process.");
            return;
        }
        Debug.Log("Start avoiding process");
        _colliderToAvoid = collision.collider;
        _avoidingRoutine = StartCoroutine(AvoidingRoutine());
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionExit2D");
        if (_isAvoiding && collision.collider == _colliderToAvoid)
        {
            StopAvoiding();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Ontrigger exit");
        if (_isAvoiding && collision == _colliderToAvoid)
        {
            StopAvoiding();
        }
    }


    private void StopAvoiding()
    {
        Debug.Log("Stop avoiding");
        _isAvoiding = false;
        if (_avoidingRoutine != null)
            StopCoroutine(_avoidingRoutine);
        if (!_pathFollower.IsRunning)
            _pathFollower.Resume();
    }


    // Check if the two vector are aiming to the same main cardinal direction
    private bool AreSameDirection(Vector2 v1, Vector2 v2)
        => Utils.GetDirection(v1) == Utils.GetDirection(v2);
    

    private IEnumerator AvoidingRoutine()
    {
        _isAvoiding = true;
        _pathFollower.Stop(); 
        yield return new WaitForSeconds(AvoidingFirstWaitingTime);
        StartCoroutine(GetAngry());
    }


    private IEnumerator GetAngry()
    {
        Debug.Log("Get angry");
        _myCollider.isTrigger = true;
        _pathFollower.EditSpeed(AngryModeSpeedMultiplier);
        _pathFollower.Resume();
        yield return new WaitForSeconds(AngryModeDuration);
        CalmDown();
    }


    private void CalmDown()
    {
        _myCollider.isTrigger = false;
        _pathFollower.ResetSpeed();
    }
    #endregion
}
