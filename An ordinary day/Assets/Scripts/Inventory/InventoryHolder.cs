using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Manage an inventory of the given game object
/// </summary>
public class InventoryHolder : MonoBehaviour
{
    [SerializeField]
    private RuntimeInventory _runtimeInventory;
    [SerializeField]
    private GameItemDataList _allGameItems; // necessary to add specific item from the item list


    public void AddItem(GameItemData item)
    {
        _runtimeInventory.AddItem(item);
    }


    public void RemoveItem(GameItemData item)
    {
        _runtimeInventory.RemoveItem(item);
    }


    [YarnCommand("addItem")]
    public void AddItem(string itemTag)
    {
        Debug.Log("Add item: " + itemTag);
        var item = _allGameItems.GetItem(itemTag);
        if (item == null)
        {
            Debug.LogError("Couldnt find any object with the tag: " + itemTag);
            return;
        }
        AddItem(item);
    }

    [YarnCommand("removeItem")]
    public void RemoveItem(string itemTag)
    {
        var item = _runtimeInventory.Value.GetItem(itemTag);
        if (item != null)
        {
            RemoveItem(item);
        }
    }


    public bool HasItem(string itemTag) => _runtimeInventory.Value.HasItem(itemTag);
}
