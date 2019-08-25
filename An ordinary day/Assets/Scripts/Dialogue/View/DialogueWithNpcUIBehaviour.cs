﻿using System.Collections;
using UnityEngine;
using Yarn;
using System.Linq;

/// <summary>
/// Manage dialogue view with NPC
/// </summary>
public class DialogueWithNpcUIBehaviour : Yarn.Unity.DialogueUIBehaviour
{
    private const KeyCode ContinueKey = KeyCode.Space;

    [SerializeField]
    private GameObject _dialogueContainer;

    [Header("Player")]
    [SerializeField]
    private PlayerDialogueDisplay _playerDisplay;
    [SerializeField]
    private PlayerData _playerData;

    [Header("NPC")]
    [SerializeField]
    private NPCDialogueDisplay _npcDisplay;
    [SerializeField]
    private NPCDataList _allNpcs;

    private CharacterDialogueDisplay CurrentDialogueDisplay
        => _playerDisplay.IsActive() ? _playerDisplay : (CharacterDialogueDisplay) _npcDisplay;
    
    private string _speaker;
    private bool _optionWasJustChosen;

    private void Awake()
    {
        Debug.Log("Dialogue Awake");
        ResetDisplays();
        _playerDisplay.SetCharacterName(_playerData.FirstName);
        _playerDisplay.SetCharacterPicture(_playerData.DialoguePicture);
        _dialogueContainer.SetActive(false);
    }

    #region Yarn Override methods

    /// Called when the dialogue system has started running.
    public override IEnumerator DialogueStarted()
    {
        Debug.Log("Dialogue starting!");
        _speaker = null;
        _playerDisplay.Show(false);
        _npcDisplay.Show(false);
        _dialogueContainer.SetActive(true);
        yield break;
    }


    /// Called when the dialogue system has finished running.
    public override IEnumerator DialogueComplete()
    {
        Debug.Log("Dialogue complete");
        yield return WaitForSpeakerToFinish();
        // Hide the dialogue interface.
        _dialogueContainer.SetActive(false);
        ResetDisplays();
        yield break;
    }


    public override IEnumerator RunCommand(Command command)
    {
        throw new System.NotImplementedException();
    }


    public override IEnumerator RunLine(Line line)
    {
        Debug.Log("Begin runLine: " + line.text);
        var lineSpeaker = ExtractSpeakerTag(ref line);
        // First line of the dialogue: 
        if (string.IsNullOrEmpty(_speaker))
        {
            Debug.Log("First speaker: " + lineSpeaker);
            ChangeSpeaker(lineSpeaker);
            yield return CurrentDialogueDisplay.SetText(line.text);
        }
        // We keep the same speaker as before, we just add some text to the previous text input
        else if (string.IsNullOrEmpty(lineSpeaker) || _speaker.Equals(lineSpeaker))
        {
            Debug.Log("Same speaker: " + _speaker);
            yield return CurrentDialogueDisplay.AppendLine(line.text);
        }
        else // we change speaker only if we reached last page of the previous dialogue
             // and the user pressed the continue key
        {
            yield return WaitForSpeakerToFinish();
            ChangeSpeaker(lineSpeaker);
            yield return CurrentDialogueDisplay.SetText(line.text);
        }
        Debug.Log("RunLine ( " + _speaker + ") :" + line.text);
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
        while (!_playerDisplay.HasChooseOption)
        {
            yield return null;
        }
        Debug.Log("Option was choosed");
        _optionWasJustChosen = true;
        // Display current dialogue line
    }
    #endregion

    private IEnumerator WaitForSpeakerToFinish()
    {
        // We wait to be sure to be at the last page of the current dialogue line
        // to dont skip dialogue lines
        while (!CurrentDialogueDisplay.IsAtLastPage())
        {
            yield return null;
        }
        // We need to wait the next frame to refresh the state of the input system
        // we already pressed the continue key to go to the last page of the current dialogue
        yield return new WaitForEndOfFrame();
        // Then, and only then, we wait for user input to display the dialogue options
        if (_optionWasJustChosen)
        {
            _optionWasJustChosen = false;
            yield break;
        }
        while (!Input.GetKeyDown(ContinueKey))
        {
            yield return null;
        }
    }

    #region Speaker Management

    /// <summary>
    /// Extract the speaker tag from the current dialogue line
    /// Remove also the tag from the line text (Line is here passed by reference)
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private string ExtractSpeakerTag(ref Line line)
    {
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
        }
        else
        {
            var npcData = _allNpcs.Items.FirstOrDefault(npc => npc.FirstName.Equals(_speaker));
            // some check safety here on the npc tag
            NpcSpeaks(npcData);
        }
    }


    private void PlayerSpeaks()
    {
        _playerDisplay.Show(true);
        _npcDisplay.Show(false);
    }


    private void NpcSpeaks(NPCData npcData)
    {
        _playerDisplay.Show(false);
        _npcDisplay.Show(true);
        _npcDisplay.SetCharacterName(npcData.FirstName);
        _npcDisplay.SetCharacterPicture(npcData.DialoguePicture);
    }

    #endregion

    private void ResetDisplays()
    {
        _playerDisplay.Reset();
        _npcDisplay.Reset();
    }
}
