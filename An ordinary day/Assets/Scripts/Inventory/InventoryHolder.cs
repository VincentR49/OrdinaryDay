using UnityEngine;
using System.Linq;
using Yarn.Unity;

/// <summary>
/// Manage an inventory of the given game object
/// </summary>
public class InventoryHolder : MonoBehaviour
{
    [SerializeField]
    private RuntimeInventory _runtimeInventory;
    [SerializeField]
    private GameItemDataList _allGameItems;

    [YarnCommand("addItem")]
    public void AddItem(string itemTag)
    {
        Debug.Log("Add item: " + itemTag);
        var item = GetItem(itemTag);
        if (item == null)
        {
            Debug.LogError("Couldnt find any object with the tag: " + itemTag);
            return;
        }
        _runtimeInventory.AddItem(item);
    }

    [YarnCommand("removeItem")]
    public void RemoveItem(string itemTag)
    {
        var item = GetItem(itemTag);
        if (item == null)
        {
            Debug.LogError("Couldnt find any object with the tag: " + itemTag);
            return;
        }
        _runtimeInventory.RemoveItem(item);
    }


    public bool HasItem(string itemTag) => _runtimeInventory.Value.HasItem(itemTag);

    private GameItemData GetItem(string itemTag) => _allGameItems.GetItem(itemTag);
}
