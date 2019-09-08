using UnityEngine;

/// <summary>
/// Behaviour to attach in an item slot of the inventory
/// </summary>
public class PlayerInventoryItemContainer : MonoBehaviour
{
    [Header("Item prefab")]
    [SerializeField]
    private PlayerInventoryItemDisplay _itemPrefab;

    private PlayerInventoryItemDisplay _itemDisplay;
	public bool IsEmpty => _itemDisplay == null;
	public GameItemData GetItemData() => IsEmpty ? null : _itemDisplay.GetData();
    public int Index => transform.GetSiblingIndex();

    private void Awake()
    {
        Clear();
        Debug.Log(Index);
    }


    public void Clear()
	{
		foreach (Transform child in transform)
			Destroy(child.gameObject);
		_itemDisplay = null;
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
		_itemDisplay = Instantiate(_itemPrefab, transform);
		_itemDisplay.Init(itemData);
	}
}
