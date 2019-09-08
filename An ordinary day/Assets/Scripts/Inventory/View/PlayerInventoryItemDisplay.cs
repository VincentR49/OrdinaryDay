using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage the diplay of an inventory object
/// </summary>
public class PlayerInventoryItemDisplay : MonoBehaviour
{
    [SerializeField]
    private Image _itemPicture;

    private GameItemData _itemData;

    public void Init(GameItemData gameItem)
    {
        _itemData = gameItem;
        _itemPicture.sprite = gameItem.Sprite;
        _itemPicture.enabled = true;
    }


    public void Reset()
    {
        _itemData = null;
        _itemPicture.sprite = null;
        _itemPicture.enabled = false;
    }
}
