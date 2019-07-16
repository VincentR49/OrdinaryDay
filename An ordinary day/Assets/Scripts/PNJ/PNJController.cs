using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manage the PNJ behaviours
/// </summary>
public class PNJController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private PNJData _pnjData;

    [Header("Managers")]
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;

    [Header("PathFinding")]
    [SerializeField]
    private PathFinder _pathFinder;
    [SerializeField]
    private PathFollower _pathFollower;

    private Collider2D _pnjCollider;

    [Header("PathFinding Debug")]
    [SerializeField]
    private Transform _pathDebugTarget;

    #region Init
    private void Awake()
    {
        Debug.Log("PNJ Creation: " + _pnjData);
        _pnjCollider = GetComponent<Collider2D>();
        InitSprites();
    }

    private void Start()
    {
        InitPathFindingSystem();
    }


    private void InitSprites()
    {
        _spriteDirectioner.SetCardinalSprite(_pnjData.CardinalSprite);
        _walkManager.SetWalkAnimation(_pnjData.WalkingAnimation);
    }
    #endregion

    #region Path following

    private void InitPathFindingSystem()
    {
        var colliderScanner = FindObjectOfType<ColliderScanner>();
        if (colliderScanner == null)
        {
            Debug.LogError("Collider scanner not found !");
            return;
        }
        _pathFinder.SetColliderScanner(colliderScanner);
        Debug.Log(_pnjData.FirstName + " pathfinding system initialized");
    }


    private void GoToTarget(Vector2 target)
    {
        var path = _pathFinder.FindShortestPath(transform.position, target, new Collider2D[]{ _pnjCollider});
        if (path == null)
        {
            Debug.LogError("Cannot go to target " + target);
            return;
        }
        var pathQueue = new Queue<Vector2>(path);
        _pathFollower.FollowPath(pathQueue);
    }

    #endregion


    #region Debug

    public void GoToDebugTarget()
    {
        GoToTarget(_pathDebugTarget.position);
    }

    #endregion
}
