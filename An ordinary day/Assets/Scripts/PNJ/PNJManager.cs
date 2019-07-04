using UnityEngine;

/// <summary>
/// Manage the PNJ behaviours
/// </summary>
public class PNJManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private PNJData _pnjData;

    [Header("Managers")]
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;


    private void Awake()
    {
        Debug.Log("PNJ Creation: " + _pnjData);
        _spriteDirectioner.SetCardinalSprite(_pnjData.CardinalSprite);
        _walkManager.SetWalkAnimation(_pnjData.WalkingAnimation);
    }
}
