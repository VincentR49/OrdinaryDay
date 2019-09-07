using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manage the display of the player inventory
/// </summary>
public class PlayerInventoryDisplay : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private RuntimeInventory _playerInventory;

    [Header("View")]
    [SerializeField]
    private GameObject _inventoryContainer;
    [SerializeField]
    private GridLayoutGroup _itemsGrid;

    [Header("Item prefab")]
    [SerializeField]
    private PlayerInventoryItemDisplay _itemPrefab;

    [Header("Debug")]
    [SerializeField]
    private bool _hideOnAwake;

    public bool IsOpen { get; private set; }
    private List<ItemContainer> _itemContainers;


    private class ItemContainer
	{
		public Transform transform;
		private PlayerInventoryItemDisplay _itemDisplay;
		public bool IsEmpty => _itemDisplay == null;
        public GameItemData GetItemData() => IsEmpty ? null : _itemDisplay.GetData();

		public ItemContainer(Transform transform)
		{
			this.transform = transform;
		}

        public void Clear()
		{
			foreach (Transform child in transform)
				Destroy(child.gameObject);
			_itemDisplay = null;
		}

        public void AddItem(PlayerInventoryItemDisplay itemPrefab, GameItemData itemData)
		{
            if (!IsEmpty)
			{
				Debug.LogError("Cannot Add item, an item is already present.");
				return;
			}
            _itemDisplay = Instantiate(itemPrefab, transform);
            _itemDisplay.Init(itemData);
        }
	}


    #region Init
    private void Awake()
    {
        if (_hideOnAwake)
            Hide();
        InitContainers();
        AddInventoryListeners();
    }


    private void Start()
    {
        InitItemDisplays();
    }


    private void InitContainers()
    {
        _itemContainers = new List<ItemContainer>();
        foreach (Transform containerTransform in _itemsGrid.transform)
        {
			var container = new ItemContainer(containerTransform);
			container.Clear();
            _itemContainers.Add(container);
        }
    }


    private void InitItemDisplays()
    {
        if (_playerInventory.Value == null)
            return;
        foreach (var item in _playerInventory.Value.GetItems())
        {
            AddItem(item);
        }
    }
    #endregion



    private void OnDestroy()
    {
        RemoveInventoryListeners();
    }



    #region Inventory listeners

    private void AddInventoryListeners()
    {
        _playerInventory.OnItemAdded += OnItemAdded;
        _playerInventory.OnItemRemoved += OnItemRemoved;
    }

    private void RemoveInventoryListeners()
    {
        _playerInventory.OnItemAdded -= OnItemAdded;
        _playerInventory.OnItemRemoved -= OnItemRemoved;
    }


    private void OnItemAdded(GameItemData itemData)
    {
        AddItem(itemData);
    }


    private void OnItemRemoved(GameItemData itemData)
    {
        RemoveItem(itemData);
    }
    #endregion


    #region Update view

    private void MoveItem(int originalIndex, int newIndex)
	{
        // TODO
	}


    private void AddItem(GameItemData itemData)
    {
        var container = GetFirstFreeContainer();
        container.AddItem(_itemPrefab, itemData);
    }


    private void RemoveItem(GameItemData itemData)
    {
		var index = _itemContainers.FindIndex(x => x.GetItemData() == itemData);
        _itemContainers[index].Clear();
        // TODO
    }


    private ItemContainer GetFirstFreeContainer()
    {
        foreach (var container in _itemContainers)
        {
            if (container.IsEmpty)
                return container;
        }
        return null;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (IsOpen)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public void Show()
    {
        Debug.Log("Show player inventory");
        _inventoryContainer.SetActive(true);
        IsOpen = true;
    }


    public void Hide()
    {
        Debug.Log("Hide player inventory");
        _inventoryContainer.SetActive(false);
        IsOpen = false;
    }
    #endregion

}
