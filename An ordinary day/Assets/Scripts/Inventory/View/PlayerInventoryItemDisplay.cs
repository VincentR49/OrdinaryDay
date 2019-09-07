using UnityEngine;
using UnityEngine.UI;


public class PlayerInventoryItemDisplay : MonoBehaviour
{
    [SerializeField]
    private Image _itemPicture;

    private GameItemData _itemData;

    public void Init(GameItemData gameItem)
    {
        _itemData = gameItem;
        _itemPicture.sprite = gameItem.Sprite;
    }

    public GameItemData GetData() => _itemData;
}
