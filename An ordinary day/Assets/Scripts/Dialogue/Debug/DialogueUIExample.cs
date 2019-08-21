using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;

public class DialogueUIExample : Yarn.Unity.DialogueUIBehaviour
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private List<Button> _optionButtons;

    [SerializeField]
    private GameObject _dialogueContainer;

    [Header("Debug")]
    [SerializeField]
    private GameObject _restartButtonContainer;

    /// A delegate (ie a function-stored-in-a-variable) that
    /// we call to tell the dialogue system about what option
    /// the user selected
    private Yarn.OptionChooser SetSelectedOption;

    private void Awake()
    {
        _text.gameObject.SetActive(false);
        foreach (var button in _optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }


    public override IEnumerator RunCommand(Command command)
    {
        // todo
        throw new System.NotImplementedException();
    }


    public override IEnumerator RunLine(Line line)
    {
        // todo do more stuff here
        // Show the text
        _text.gameObject.SetActive(true);
        _text.text = line.text;
        yield return null;
    }

    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        // Do a little bit of safety checking
        if (optionsCollection.options.Count > _optionButtons.Count)
        {
            Debug.LogWarning("There are more options to present than there are" +
                             "buttons to present them in. This will cause problems.");
        }

        // Display each option in a button, and make it visible
        int i = 0;
        foreach (var optionString in optionsCollection.options)
        {
            _optionButtons[i].gameObject.SetActive(true);
            _optionButtons[i].GetComponentInChildren<Text>().text = optionString;
            i++;
        }

        // Record that we're using it
        SetSelectedOption = optionChooser;

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (SetSelectedOption != null)
        {
            yield return null;
        }

        // Hide all the buttons
        foreach (var button in _optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }


    /// Called by buttons to make a selection.
    public void SetOption(int selectedOption)
    {
        // Call the delegate to tell the dialogue system that we've
        // selected an option.
        SetSelectedOption(selectedOption);

        // Now remove the delegate so that the loop in RunOptions will exit
        SetSelectedOption = null;
    }


    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted()
    {
        Debug.Log("Dialogue starting!");
        // Enable the dialogue controls.
        if (_dialogueContainer != null)
            _dialogueContainer.SetActive(true);
        if (_restartButtonContainer != null)
            _restartButtonContainer.SetActive(false);
        yield break;
    }

    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        Debug.Log("Complete!");
        if (_restartButtonContainer != null)
            _restartButtonContainer.SetActive(true);

        // Wait for any user input
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        // Hide the dialogue interface.
        if (_dialogueContainer != null)
            _dialogueContainer.SetActive(false);
        yield break;
    }
}
