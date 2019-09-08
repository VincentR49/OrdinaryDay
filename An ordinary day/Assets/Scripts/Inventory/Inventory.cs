using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField]
    private List<GameItemData> _items = new List<GameItemData>();
    [SerializeField]
    private int _money;

    public delegate void ItemMovementHandler(GameItemData item);
    public event ItemMovementHandler OnItemAdded;
    public event ItemMovementHandler OnItemRemoved;


    public Inventory()
    {
        // nothing yet
    }

    public Inventory(Inventory other)
    {
        _money = other._money;
        _items = new List<GameItemData>();
        foreach (var item in other._items)
            _items.Add(item);
    }

    public void AddItem(GameItemData item)
    {
        if (_items.Contains(item) && item.IsUnique)
        {
            Debug.LogWarning("Item unique already in inventory: " + item.Tag);
            return;
        }
        Debug.Log("Add Item in inventory: " + item.Tag);
        _items.Add(item);
        OnItemAdded?.Invoke(item);
    }


    public void RemoveItem(GameItemData item)
    {
        _items.Remove(item);
        OnItemRemoved?.Invoke(item);
        Debug.Log("Remove Item from inventory: " + item.Tag);
    }


    public void SetMoney(int amount) => _money = amount;
    public void AddAmount(int amount) => _money += amount;
    public int GetMoney() => _money;


    public List<GameItemData> GetItems() => _items;
}