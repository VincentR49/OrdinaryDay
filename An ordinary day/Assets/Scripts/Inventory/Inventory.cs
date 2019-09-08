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
        _items.Add(item);
    }


    public void RemoveItem(GameItemData item)
    {
        _items.Remove(item);
    }


    public void SetMoney(int amount) => _money = amount;
    public void AddAmount(int amount) => _money += amount;
    public int GetMoney() => _money;


    public List<GameItemData> GetItems() => _items;
}