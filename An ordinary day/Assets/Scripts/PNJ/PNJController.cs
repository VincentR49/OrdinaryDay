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
        Debug.Log("Init Schedule System of " + _pnjData.FirstName);
        _scheduleHandler.Init(_pnjData.Schedule);
    }
    #endregion
}
