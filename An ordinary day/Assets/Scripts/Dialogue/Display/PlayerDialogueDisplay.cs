using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn;

/// <summary>
/// Manage the player relative dialogue display
/// </summary>
public class PlayerDialogueDisplay : CharacterDialogueDisplay
{
    [Header("Options")]
    [SerializeField]
    private GameObject _optionsContainer;
    [SerializeField]
    private List<Button> _dialogueOptions;

    /// A delegate (ie a function-stored-in-a-variable) that
    /// we call to tell the dialogue system about what option
    /// the user selected (Yarn stuff)
    private Yarn.OptionChooser SetSelectedOption;
    public bool HasChoosenAnOption { private set; get; }
    private PlayerData _playerData;


    public void Init(PlayerData playerData)
    {
        _picture.sprite = playerData.DialoguePicture;
        _name.text = playerData.FirstName;
    }


    public void ShowOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        Reset();
        HideAllOptionButtons();
        _optionsContainer.SetActive(true);
        HasChoosenAnOption = false;
        SetSelectedOption = optionChooser;
        int count = 0;
        foreach (var option in optionsCollection.options)
        {
            _dialogueOptions[count].GetComponentInChildren<TextMeshProUGUI>().text = option;
            _dialogueOptions[count].gameObject.SetActive(true);
            count++;
        }
    }


    /// Called by buttons to make a selection (attach in inspector)
    public void SetOption(int selectedOption)
    {
        Debug.Log("Set Option: " + selectedOption);
        SetSelectedOption(selectedOption);
        // Now remove the delegate so that the loop in RunOptions will exit
        HasChoosenAnOption = true;
        _optionsContainer.SetActive(false);
        HideAllOptionButtons();
    }

    

    private void HideAllOptionButtons()
    {
        foreach (var button in _dialogueOptions)
            button.gameObject.SetActive(false);
    }
}
