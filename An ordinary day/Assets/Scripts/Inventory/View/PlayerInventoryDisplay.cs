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
    [SerializeField]
    private PlayerInventoryItemInfoDisplay _itemInfoDisplay;
    
    [Header("Debug")]
    [SerializeField]
    private bool _hideOnAwake;

    private List<PlayerInventoryItemContainer> _itemContainers;

    public bool IsOpen => _inventoryPanel.activeSelf;

    #region Init
    private void Awake()
    {
        HideObjectInfo();
        if (_hideOnAwake)
            Hide();
        RefreshContainersList();
        AddContainersListeners();
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
        RemoveContainersListeners();
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


    private void OnItemPointerEnter(GameItemData itemData)
    {
        //Debug.Log("OnItemPointerEnter");
        _itemInfoDisplay.Init(itemData);
        var container = _itemContainers.FirstOrDefault(x => x.ContainObject(itemData));
        ShowObjectInfo(container.transform.position);
    }


    private void OnItemPointerExit(GameItemData itemData)
    {
        HideObjectInfo();
    }


    private void RemoveContainersListeners()
    {
        foreach (var container in _itemContainers)
        {
            container.OnItemSelected -= OnItemPointerEnter;
            container.OnItemUnselected -= OnItemPointerExit;
        }
    }


    private void AddContainersListeners()
    {
        foreach (var container in _itemContainers)
        {
            container.OnItemSelected += OnItemPointerEnter;
            container.OnItemUnselected += OnItemPointerExit;
        }
    }
    #endregion


    #region Containers management

    private void AddItem(GameItemData itemData)
    {
        var container = GetFirstFreeContainer();
        container.AddItem(itemData);
    }


    private void RemoveItem(GameItemData itemData)
    {
		var container = _itemContainers.FirstOrDefault(x => x.ContainObject(itemData));
        container.RemoveItem();
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

    #endregion


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
    }


    public void Hide()
    {
        Debug.Log("Hide player inventory");
        _inventoryPanel.SetActive(false);
        HideObjectInfo();
        foreach (var container in _itemContainers)
            container.UnSelectContainer();
    }



    private void ShowObjectInfo(Vector3 position)
    {
        _itemInfoDisplay.transform.position = position;
        _itemInfoDisplay.Show(true);
    }


    private void HideObjectInfo()
    {
        _itemInfoDisplay.Show(false);
    }
}
