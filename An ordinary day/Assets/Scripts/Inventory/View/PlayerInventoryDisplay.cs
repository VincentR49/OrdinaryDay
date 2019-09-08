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
    private GameObject _inventoryPanel;
    [SerializeField]
    private GridLayoutGroup _containersGrid;
    
    [Header("Debug")]
    [SerializeField]
    private bool _hideOnAwake;

    private List<PlayerInventoryItemContainer> _itemContainers;

    public bool IsOpen { get; private set; }

    #region Init
    private void Awake()
    {
        if (_hideOnAwake)
            Hide();
        RefreshContainersList();
        AddInventoryListeners();
    }


    private void Start()
    {
        InitItemDisplays();
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

    private void AddItem(GameItemData itemData)
    {
        var container = GetFirstFreeContainer();
        container.AddItem(itemData);
    }


    private void RemoveItem(GameItemData itemData)
    {
		var container = _itemContainers.FirstOrDefault(x => x.GetItemData() == itemData);
        container.Clear();
        container.MoveAtTheEnd();
        RefreshContainersList();
        foreach (var y in _itemContainers)
            Debug.Log(y.Index);
    }


    private PlayerInventoryItemContainer GetFirstFreeContainer()
    {
        foreach (var container in _itemContainers)
        {
            if (container.IsEmpty)
                return container;
        }
        return null;
    }


    private void RefreshContainersList()
    {
        _itemContainers = new List<PlayerInventoryItemContainer>();
        foreach (Transform child in _containersGrid.transform)
        {
            _itemContainers.Add(child.GetComponent<PlayerInventoryItemContainer>());
        }
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
        _inventoryPanel.SetActive(true);
        IsOpen = true;
    }


    public void Hide()
    {
        Debug.Log("Hide player inventory");
        _inventoryPanel.SetActive(false);
        IsOpen = false;
    }
    #endregion

}
