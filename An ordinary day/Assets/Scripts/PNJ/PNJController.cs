using UnityEngine;

/// <summary>
/// Manage the PNJ behaviours
/// </summary>
public class PNJController : MonoBehaviour
{
    [SerializeField]
    private PNJControllerList _pnjControllerList;

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


    private void OnDestroy()
    {
        _pnjControllerList.Remove(this);
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
        _pnjControllerList.Add(this); // we register only if initialized
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

    public PNJData GetPNJData() => _pnjData;
}
