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

    public void Init()
    {
        Init(new Inventory());
    }

    public void Init(InventoryData other)
    {
        Init(other.Value);
    }

    public void Init(Inventory other)
    {
        if (Value != null)
            RemovedInventoryListener();
        Value = new Inventory(other);
        AddInventoryListener();
    }


    public void AddItem(GameItemData item)
    {
        Value.AddItem(item);
    }


    public void RemoveItem(GameItemData item)
    {
        Value.RemoveItem(item);
    }


    private void AddInventoryListener()
    {
        Value.OnItemAdded += FireOnItemAddedEvent;
        Value.OnItemRemoved += FireOnItemRemovedEvent;
    }

    private void RemovedInventoryListener()
    {
        Value.OnItemAdded -= FireOnItemAddedEvent;
        Value.OnItemRemoved -= FireOnItemRemovedEvent;
    }

    private void FireOnItemAddedEvent(GameItemData itemData) => OnItemAdded?.Invoke(itemData);
    private void FireOnItemRemovedEvent(GameItemData itemData) => OnItemRemoved?.Invoke(itemData);
}
