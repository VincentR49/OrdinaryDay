using UnityEngine;
using System.Collections.Generic;
using Yarn.Unity;

/// <summary>
/// Storing variable system for npc dialogue
/// </summary>
public class DialogueVariableStorage : VariableStorageBehaviour
{
    [SerializeField]
    private GameItemDataList _allGameItems;

    [Header("Player")]
    [SerializeField]
    private PlayerData _playerData;

    /// Where we actually keeping our variables
    private Dictionary<string, Yarn.Value> _variables = new Dictionary<string, Yarn.Value>();

    /// Reset to our default values when the game starts
    private void Awake()
    {
        ResetToDefaults();
        AddInventoryListeners();
    }

    private void OnDestroy()
    {
        RemoveInventoryListeners();
    }

    /// Erase all variables and reset to default values
    public override void ResetToDefaults()
    {
        Clear();
        _variables.Add(DialogueVariableConstants.PlayerName, new Yarn.Value(_playerData.FirstName + " " + _playerData.LastName));
        _variables.Add(DialogueVariableConstants.KnowJisooHouseKeyLocation, new Yarn.Value(false)); // debug (otherwise false)
        RefreshPlayerInventoryRelatedVariables();
    }

    #region Player Inventory

    private void AddInventoryListeners()
    {
        _playerData.Inventory.OnItemAdded += OnPlayerInventoryChanged;
        _playerData.Inventory.OnItemRemoved += OnPlayerInventoryChanged;
    }


    private void RemoveInventoryListeners()
    {
        _playerData.Inventory.OnItemAdded -= OnPlayerInventoryChanged;
        _playerData.Inventory.OnItemRemoved -= OnPlayerInventoryChanged;
    }


    private void RefreshPlayerInventoryRelatedVariables()
    {
        Debug.Log("RefreshPlayerInventoryRelatedVariables");
        foreach (var item in _allGameItems.Items)
        {
            RefreshPlayerInventoryRelatedVariables(item.Tag);
        }
    }

    private void RefreshPlayerInventoryRelatedVariables(string itemTag)
    {
        var playerHasItem = _playerData.Inventory.Value.HasItem(itemTag);
        var variableKey = DialogueVariableConstants.GetHasItemVariable(itemTag, _playerData.DialogueTag);
        if (_variables.ContainsKey(variableKey))
        {
            _variables.Remove(variableKey); // error otherwise
        }
        _variables.Add(variableKey, new Yarn.Value(playerHasItem));
    }

    private void OnPlayerInventoryChanged(GameItemData gameItem)
    {
        Debug.Log("OnPlayerInventoryChanged");
        RefreshPlayerInventoryRelatedVariables(gameItem.Tag);
    }
    #endregion



    /// Set a variable's value
    public override void SetValue(string variableName, Yarn.Value value)
    {
        // Copy this value into our list
        _variables[variableName] = new Yarn.Value(value);
        //Debug.Log("Set variable: " + variableName + ": " + value);
    }

    /// Get a variable's value
    public override Yarn.Value GetValue(string variableName)
    {
        // If we don't have a variable with this name, return the null value
        if (_variables.ContainsKey(variableName) == false)
            return Yarn.Value.NULL;
        return _variables[variableName];
    }

    /// Erase all variables
    public override void Clear()
    {
        _variables.Clear();
    }
}
