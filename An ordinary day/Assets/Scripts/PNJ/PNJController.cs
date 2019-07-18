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
   
   
    #region Init
    private void Awake()
    {
        Debug.Log("PNJ Creation: " + _pnjData);
        InitSprites();
    }

    private void InitSprites()
    {
        _spriteDirectioner.SetCardinalSprite(_pnjData.CardinalSprite);
        _walkManager.SetWalkAnimation(_pnjData.WalkingAnimation);
    }
    #endregion
}
