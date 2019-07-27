using UnityEngine;

/// <summary>
/// Manage the PNJ behaviours
/// </summary>
public class PNJController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private ScheduleHandler _scheduleHandler;

    private PNJData _pnjData;

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
        _scheduleHandler.SetSchedule(_pnjData.InGameSchedule);
    }
    #endregion
}
