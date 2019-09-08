using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour to attach in an item slot of the inventory
/// </summary>
public class PlayerInventoryItemContainer : MonoBehaviour
{
    [Header("Item")]
    [SerializeField]
    private PlayerInventoryItemDisplay _itemDisplay;

    [Header("Design")]
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private Color _pointerOverColor;

	public bool IsEmpty => _itemData == null;
    public int Index => transform.GetSiblingIndex();
    private Color _originalColor;
    private GameItemData _itemData;

    public delegate void OnItemSelection(GameItemData itemData);
    public event OnItemSelection OnItemSelected;
    public event OnItemSelection OnItemUnselected;


    private void Awake()
    {
        _originalColor = _backgroundImage.color;
        RemoveItem();
    }


    public void RemoveItem()
	{
        _itemData = null;
        _itemDisplay.Reset();
	}


    public void MoveAtTheEnd()
    {
        transform.SetAsLastSibling();
    }


    public void AddItem(GameItemData itemData)
	{
		if (!IsEmpty)
		{
			Debug.LogError("Cannot Add item, an item is already present.");
			return;
		}
        _itemData = itemData;
		_itemDisplay.Init(itemData);
	}


    public void OnPointerEnter()
    {
        //Debug.Log("OnPointerEnter");
        if (IsEmpty)
            return;
        SelectContainer();
        OnItemSelected?.Invoke(_itemData);
    }


    public void OnPointerExit()
    {
        //Debug.Log("OnPointerExit");
        UnSelectContainer();
        OnItemUnselected?.Invoke(_itemData);
        if (IsEmpty)
            return;
    }


    public void SelectContainer()
    {
        _backgroundImage.color = _pointerOverColor;
    }


    public void UnSelectContainer()
    {

        _backgroundImage.color = _originalColor;
    }


    public bool ContainObject(GameItemData itemData) => IsEmpty ? false : itemData == _itemData;
}
