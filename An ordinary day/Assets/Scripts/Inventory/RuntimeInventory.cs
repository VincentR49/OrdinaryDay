using UnityEngine;

/// <summary>
/// Use this class in order to store inventory runtime data (dynamic inventory)
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Inventory/Runtime")]
public class RuntimeInventory : RuntimeVariableData<Inventory>
{
    public delegate void ItemMovementHandler(GameItemData item);
    public event ItemMovementHandler OnItemAdded;
    public event ItemMovementHandler OnItemRemoved;


    public void Init(InventoryData other)
    {
        Value = new Inventory(other.Value);
    }


    public void AddItem(GameItemData item)
    {
        if (Value == null)
            Value = new Inventory();
        Value.AddItem(item);
        OnItemAdded?.Invoke(item);
        Debug.Log("Add Item in inventory: " + item.Tag);
    }


    public void RemoveItem(GameItemData item)
    {
        Value.RemoveItem(item);
        OnItemRemoved?.Invoke(item);
        Debug.Log("Remove Item from inventory: " + item.Tag);
    }
}
