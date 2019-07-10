using UnityEngine;

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

    [Header("PathFinding Debug")]
    [SerializeField]
    private Transform _pathTarget;
    private bool _pathfindLaunched;

    #region Init
    private void Awake()
    {
        Debug.Log("PNJ Creation: " + _pnjData);
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
    #endregion


    private void Update()
    {
        // todo some stuff here
        if (!_pathfindLaunched)
        {
            GoToTarget(_pathTarget.position); // todo debug only
            _pathfindLaunched = true;
        }
    }



    #region Path following

    private void GoToTarget(Vector2 target)
    {
        var path = _pathFinder.FindShortestPath(transform.position, target);
        if (path == null)
        {
            Debug.LogError("Cannot go to target " + target);
            return;
        }
        _pathFollower.FollowPath(path);
    }

    #endregion
}
