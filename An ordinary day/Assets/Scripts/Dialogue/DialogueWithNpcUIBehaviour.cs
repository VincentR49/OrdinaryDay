using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manage dialogue view with NPC
/// </summary>
public class DialogueWithNpcUIBehaviour : Yarn.Unity.DialogueUIBehaviour
{
    [SerializeField]
    private GameObject _dialogueContainer;

    [Header("NPC Info")]
    [SerializeField]
    private TextMeshProUGUI _npcName;
    [SerializeField]
    private Image _npcPicture;
    [SerializeField]
    private PNJDataList _allNpcDataList;

    [Header("Dialogue Display")]
    [SerializeField]
    private TextMeshProUGUI _dialogueText;
    [SerializeField]
    private List<Button> _optionButtons;


    private PNJController _currentNPC;

    /// A delegate (ie a function-stored-in-a-variable) that
    /// we call to tell the dialogue system about what option
    /// the user selected (Yarn stuff)
    private Yarn.OptionChooser SetSelectedOption;


    private void Awake()
    {
        _dialogueContainer.SetActive(false);
        foreach (var button in _optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    #region Yarn Override methods
    public override IEnumerator RunCommand(Command command)
    {
        throw new System.NotImplementedException();
    }


    public override IEnumerator RunLine(Line line)
    {
        // todo do more stuff here
        // Show the text
        Debug.Log("RunLine: " + line.text);
        _dialogueText.text += line.text + System.Environment.NewLine;
        // TODO Change with emotion
        UpdatePNJPicture();
        yield return null;
    }


    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        // Do a little bit of safety checking
        if (optionsCollection.options.Count > _optionButtons.Count)
        {
            Debug.LogError("There are more options to present than there are" +
                             "buttons to present them in. This will cause problems.");
        }

        // Display each option in a button, and make it visible
        int i = 0;
        foreach (var optionString in optionsCollection.options)
        {
            _optionButtons[i].gameObject.SetActive(true);
            _optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = optionString;
            i++;
        }

        // Record that we're using it
        SetSelectedOption = optionChooser;

        // Wait until the chooser has been used and then removed (see SetOption below)
        while (SetSelectedOption != null)
            yield return null;
        ResetText();
        // Hide all the buttons
        foreach (var button in _optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
    #endregion

    private void UpdatePNJPicture()
    {
        var npcData = _currentNPC.GetPNJData();
        _npcPicture.sprite = npcData.DialoguePicture;
        _npcName.text = npcData.FirstName;
    }


    /// Called by buttons to make a selection (attach in inspector)
    public void SetOption(int selectedOption)
    {
        SetSelectedOption(selectedOption);
        // Now remove the delegate so that the loop in RunOptions will exit
        SetSelectedOption = null;
    }


    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted()
    {
        Debug.Log("Dialogue starting!");
        _dialogueContainer.SetActive(true);
        ResetText();
        yield break;
    }


    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        Debug.Log("Dialogue complete");
        // Hide the dialogue interface.
        _dialogueContainer.SetActive(false);
        yield break;
    }


    public void SetCurrentNPC(PNJController npc)
    {
        _currentNPC = npc;
    }


    private void ResetText()
    {
        _dialogueText.text = "";
    }
}
