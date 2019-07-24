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
    [SerializeField]
    private TargetReacher _targetReacher;
    [SerializeField]
    private ScheduleHandler _scheduleHandler;
   
    #region Init
    private void Awake()
    {
        Debug.Log("PNJ Creation: " + _pnjData);
        Init();
    }


    private void Init()
    {
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
        // We use only the ingame schedule
        // The default schedule is just readonly
        Debug.Log("Init Schedule System of " + _pnjData.FirstName);
        var defaultSchedule = _pnjData.DefaultSchedule;
        var inGameSchedule = _pnjData.InGameSchedule;
        inGameSchedule.Copy(defaultSchedule);
        inGameSchedule.Reset();
        _scheduleHandler.Init(inGameSchedule);
    }
    #endregion
}
