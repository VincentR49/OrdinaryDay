using UnityEngine;

/// <summary>
/// Manage the PNJ behaviours
/// </summary>
public class PNJController : MonoBehaviour
{
    [SerializeField]
    private PNJControllerList _pnjList;

    [Header("Managers")]
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private ScheduleHandler _scheduleHandler;
    [SerializeField]
    private PNJData _pnjData;

    [Header("Debug")]
    [SerializeField]
    private bool _initOnStart;


    private void Awake()
    {
        _pnjList.Add(this);
    }

    private void OnDestroy()
    {
        _pnjList.Remove(this);
    }


    private void Start()
    {
        if (_initOnStart)
            Init(_pnjData);
    }

    #region Init
    public void Init(PNJData pnjData)
    {
        _pnjData = pnjData;
        Debug.Log("PNJ Initialisation: " + _pnjData);
        InitSprites();
        InitScheduleSystem();
    }


    private void InitSprites()
    {
        _spriteDirectioner.SetCardinalSprite(_pnjData.CardinalSprite);
        _walkManager.SetWalkAnimation(_pnjData.WalkingAnimation);
    }


    private void InitScheduleSystem()
    {
        _scheduleHandler.Init(_pnjData.InGameSchedule);
    }
    #endregion


    #region Accessors

    public PNJData GetPNJData() => _pnjData;


    #endregion
}
