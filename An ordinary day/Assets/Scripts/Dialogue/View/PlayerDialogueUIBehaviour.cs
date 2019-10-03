using System.Collections;
using UnityEngine;
using Yarn;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Manage dialogue view from player based dialogue (dialogue with npc, objects, rewards, ...)
/// </summary>
public class PlayerDialogueUIBehaviour : Yarn.Unity.DialogueUIBehaviour
{
    private const KeyCode ContinueKey = KeyCode.Space;

    [SerializeField]
    private GameObject _dialogueContainer;

    [Header("Player")]
    [SerializeField]
    private PlayerDialogueDisplay _playerDisplay;
    [SerializeField]
    private PlayerData _playerData;

    [Header("Other")]
    [SerializeField]
    private CharacterDialogueDisplay _otherDisplay;
    [SerializeField]
    private DialogueAgentDataList _allDialogueAgentData;

    [Header("Parameters")]
    [SerializeField]
    private bool _pauseGameDuringDialogue = true;

    private DialogueVariableStorage _variableStorage;
    private CharacterDialogueDisplay CurrentDialogueDisplay => _playerDisplay.IsActive() ? _playerDisplay : _otherDisplay;
    
    private string _speaker;
    private bool _optionWasJustChosen;


    private enum Speaker
    {
        Player,
        NPC,
        Object
    }

    private void Awake()
    {
        Debug.Log("Dialogue Awake");
        ResetDisplays();
        _playerDisplay.Init(_playerData);
        _dialogueContainer.SetActive(false);
    }


    private void Start()
    {
        _variableStorage = FindObjectOfType<DialogueVariableStorage>();
    }

    #region Yarn Override methods

    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted()
    {
        Debug.Log("Dialogue starting!");
        _speaker = null;
        _playerDisplay.Show(false);
        _otherDisplay.Show(false);
        _dialogueContainer.SetActive(true);
        if (_pauseGameDuringDialogue)
            GamePauser.Pause();
        yield break;
    }


    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        Debug.Log("Dialogue complete");
        yield return WaitForSpeakerToFinish();
        _dialogueContainer.SetActive(false);
        ResetDisplays();
        if(_pauseGameDuringDialogue)
            GamePauser.Resume();
        yield break;
    }


    public override IEnumerator RunCommand(Command command)
    {
        throw new System.NotImplementedException();
    }


    public override IEnumerator RunLine(Line line)
    {
        //Debug.Log("Begin runLine: " + line.text);
        var lineSpeaker = ExtractSpeakerTag(ref line);
        line.text = ParseVariables(line.text);
        // First line of the dialogue: 
        if (string.IsNullOrEmpty(_speaker))
        {
            if (string.IsNullOrEmpty(lineSpeaker))
            {
                Debug.LogWarning("The first speaker of the dialogue is unknown. Will assign the player Tag.");
                lineSpeaker = _playerData.DialogueTag;
            }
            ChangeSpeaker(lineSpeaker);
            yield return CurrentDialogueDisplay.SetText(line.text);
        }
        // We keep the same speaker as before, we just add some text to the previous text input
        else if (string.IsNullOrEmpty(lineSpeaker) || _speaker.Equals(lineSpeaker))
        {
            if (_optionWasJustChosen) // reset the dialogue line
            {
                yield return CurrentDialogueDisplay.SetText(line.text);
            }
            else
            {
                yield return CurrentDialogueDisplay.AppendText(line.text);
            }
        }
        else // we change speaker only if we reached last page of the previous dialogue
             // and the user pressed the continue key
        {
            yield return WaitForSpeakerToFinish();
            ChangeSpeaker(lineSpeaker);
            yield return CurrentDialogueDisplay.SetText(line.text);
        }
        if (_optionWasJustChosen)
            _optionWasJustChosen = false;
        Debug.Log("RunLine (" + _speaker + "): " + line.text);
    }

 
    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        _optionWasJustChosen = false;
        Debug.Log("RunOptions");
        yield return WaitForSpeakerToFinish();
        Debug.Log("Ready to display the options");
        // The choice is made by the player, so we change display
        ChangeSpeaker(_playerData.DialogueTag);
        _playerDisplay.ShowOptions(optionsCollection, optionChooser);
        // Wait until the player has made his choice
        // The option selection is managed inside the PlayerDialogueDisplay class
        while (!_playerDisplay.HasChoosenAnOption)
            yield return null;
        _optionWasJustChosen = true;
    }
    #endregion


    #region Speaker Management

    private IEnumerator WaitForSpeakerToFinish()
    {
        // We wait to be sure to be at the last page of the current dialogue line
        // to dont skip dialogue lines
        while (!CurrentDialogueDisplay.IsAtLastPage || CurrentDialogueDisplay.IsDisplayingText)
        {
            yield return null;
        }
        // We need to wait the next frame to refresh the state of the input system
        // we already pressed the continue key to go to the last page of the current dialogue
        yield return new WaitForEndOfFrame();
        // Special management in case of an option has been chosen
        // We display directly the new content
        if (_optionWasJustChosen)
        {
            yield break;
        }
        // Then, and only then, we wait for user input to display the dialogue options
        while (!Input.GetKeyDown(ContinueKey))
        {
            yield return null;
        }
    }

    /// <summary>
    /// Extract the speaker tag from the current dialogue line
    /// Remove also the tag from the line text (Line is here passed by reference)
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private string ExtractSpeakerTag(ref Line line)
    {
        // todo use regexp instead (cleaner)
        var separatorIndex = line.text.IndexOf(':');
        if (separatorIndex > 0)
        {
            var speakerTag = line.text.Substring(0, separatorIndex);
            line.text = line.text.Replace(speakerTag + ": ", "");
            return speakerTag;
        }
        return null;
    }


    private void ChangeSpeaker(string newSpeakerTag)
    {
        if (newSpeakerTag.Equals(_speaker)) // speaker didnt change, we just skip
            return;
        Debug.Log("Change speaker to: " + newSpeakerTag);
        _speaker = newSpeakerTag;
        if (_speaker.Equals(_playerData.DialogueTag))
        {
            PlayerSpeaks();
            return;
        }
        var agentData = _allDialogueAgentData.Items.FirstOrDefault(agent => agent.Tag.Equals(_speaker));
        if (agentData != null)
        {
            OtherSpeaks(agentData.DialoguePicture, agentData.DialogueDisplayName);
            return;
        }
        Debug.LogError("Couldnt find any NPC or object with the given tag: " + _speaker + "/n" +
            "You probably needs to add the corresponding dialogue data in the all dialogue data list.");
    }

    
    private void PlayerSpeaks()
    {
        _playerDisplay.Show(true);
        _otherDisplay.Show(false);
    }


    private void OtherSpeaks(Sprite picture, string nameText = "")
    {
        _playerDisplay.Show(false);
        _otherDisplay.Init(picture, nameText);
        _otherDisplay.Show(true);
    }

    #endregion


    #region Variable Display

    private string ParseVariables(string originalText)
    {
        Regex reg = new Regex(@"\[\$(.*?)\]");
        return reg.Replace(originalText, delegate (Match m) {
            return ReplaceVariableTagByValue(m.Value);
        });
    }


    private string ReplaceVariableTagByValue(string variableTag)
    {
        // variable tag is in this form : [$Tag]
        variableTag = variableTag.Replace("[", "");
        variableTag = variableTag.Replace("]", "");
        var value = _variableStorage.GetValue(variableTag);
        if (value == Value.NULL)
        {
            Debug.LogError("Couldnt find any variable with the tag: " + variableTag + ", inside the variableStorage");
            return variableTag;
        }
        return value.AsString;
    }

    #endregion

    private void ResetDisplays()
    {
        _playerDisplay.Reset();
        _otherDisplay.Reset();
    }
}
