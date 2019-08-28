using UnityEngine;
using System.Collections.Generic;
using Yarn.Unity;

/// <summary>
/// Storing variable system for npc dialogue
/// </summary>
public class NPCDialogueVariableStorage : VariableStorageBehaviour
{
    [Header("PlayerData")]
    [SerializeField]
    private PlayerData _playerData;

    /// Where we actually keeping our variables
    private Dictionary<string, Yarn.Value> _variables = new Dictionary<string, Yarn.Value>();

    /// Reset to our default values when the game starts
    private void Awake()
    {
        ResetToDefaults();
    }

    /// Erase all variables and reset to default values
    public override void ResetToDefaults()
    {
        Clear();
        _variables.Add(DialogueVariables.PlayerName, new Yarn.Value(_playerData.FirstName + " " + _playerData.LastName));
        Debug.Log("Variable PlayerName:" + _variables);
    }

    /// Set a variable's value
    public override void SetValue(string variableName, Yarn.Value value)
    {
        // Copy this value into our list
        _variables[variableName] = new Yarn.Value(value);
        Debug.Log("Set variable: " + variableName + ": " + value);
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
