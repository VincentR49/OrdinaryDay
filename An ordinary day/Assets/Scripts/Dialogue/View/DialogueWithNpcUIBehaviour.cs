using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Manage dialogue view with NPC
/// </summary>
public class DialogueWithNpcUIBehaviour : Yarn.Unity.DialogueUIBehaviour
{
    [SerializeField]
    private GameObject _dialogueContainer;

    // Create specific class here to hold the data (pnj / player)
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
    
    private bool _speakerIsPlayer;
    private string _speaker;

    /// A delegate (ie a function-stored-in-a-variable) that
    /// we call to tell the dialogue system about what option
    /// the user selected (Yarn stuff)
    private Yarn.OptionChooser SetSelectedOption;


    private void Awake()
    {
        _playerDisplay.SetCharacterName(_playerData.FirstName);
        _playerDisplay.SetCharacterPicture(_playerData.DialoguePicture);
        _dialogueContainer.SetActive(false);
    }

    #region Yarn Override methods
    public override IEnumerator RunCommand(Command command)
    {
        throw new System.NotImplementedException();
    }


    public override IEnumerator RunLine(Line line)
    {
        var lineSpeaker = ExtractSpeakerTag(line);
        // First line of the dialogue
        if (string.IsNullOrEmpty(_speaker))
        {
            _speaker = lineSpeaker;
            Debug.Log("First speaker: " + _speaker);
            ChangeDisplay();
            yield return CurrentDialogueDisplay.SetText(line.text);
        }
        // We keep the same speaker as before
        else if (string.IsNullOrEmpty(lineSpeaker) || _speaker.Equals(lineSpeaker))
        {
            Debug.Log("Same speaker: " + _speaker);
            yield return CurrentDialogueDisplay.AppendText(line.text);
        }
        else // we change speaker
        {
            while (Input.GetKeyDown(KeyCode.Space) == false)
            {
                yield return null;
            }
            _speaker = lineSpeaker;
            Debug.Log("Change speaker: " + _speaker);
            ChangeDisplay();
            yield return CurrentDialogueDisplay.SetText(line.text);
        }
        Debug.Log("RunLine: " + line.text);
    }


    private string ExtractSpeakerTag(Line line)
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


    private void ChangeDisplay()
    {
        if (_speaker.Equals(_playerData.DialogueTag))
        {
            PlayerStartSpeaking();
        }
        else
        {
            // Check if the tag belong to a known npc
            var npcData = _allNpcs.Items.FirstOrDefault(npc => npc.FirstName.Equals(_speaker));
            NPCStartsSpeaking(npcData);
        }
    }


    private void CheckWhoIsSpeaking(Line line)
    {
        var speakerTag = line.text.Substring(0, line.text.IndexOf(':'));
        // same speaker as before
        if (string.IsNullOrEmpty(speakerTag)) 
            return;
        // player is speaking, and was not before
        if (speakerTag.Equals(_playerData.DialogueTag) && !_speakerIsPlayer) 
        {
            PlayerStartSpeaking();
        }
        else
        {
            // Check if the tag belong to a known npc
            var npcData = _allNpcs.Items.FirstOrDefault(npc => npc.FirstName.Equals(speakerTag));
            if (npcData == null)
            {
                Debug.LogError("Wrong Tag. Couldnt find any instanciated NPC having the first name: " + speakerTag);
                return;
            }
            NPCStartsSpeaking(npcData);
        }
    }


    private void PlayerStartSpeaking()
    {
        _playerDisplay.Show(true);
        _npcDisplay.Show(false);
        _speakerIsPlayer = true;
    }


    private void NPCStartsSpeaking(NPCData npcData)
    {
        // the speaker was previously the player, we switch display
        if (_speakerIsPlayer)
        {
            _playerDisplay.Show(false);
            _npcDisplay.Show(true);
        }
        _npcDisplay.SetCharacterName(npcData.FirstName);
        _npcDisplay.SetCharacterPicture(npcData.DialoguePicture);
        _speakerIsPlayer = false;
    }



    public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
    {
        while (SetSelectedOption != null)
            yield return null;
        // TODO
    }
    #endregion

   
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
        _speaker = null;
        _dialogueContainer.SetActive(true);
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
}
