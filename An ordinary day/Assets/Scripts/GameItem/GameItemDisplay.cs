using UnityEngine;


/// <summary>
/// Attach this to an object to replace its sprite by the one of the gameItem
/// </summary>
public class GameItemDisplay : MonoBehaviour
{
    [SerializeField]
    private GameItemData _gameItem;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;


    public void RefreshDisplay()
    {
        _spriteRenderer.sprite = _gameItem.Sprite;
    }
}
