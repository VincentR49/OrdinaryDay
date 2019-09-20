using UnityEngine;

/// <summary>
/// Data holder of Dialogue Information for Yarn Plugin
/// Use this one for door kind of dialogue.
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Dialogue/Door")]
public class DoorDialogueData : DialogueAgentData
{
    private const string LockedDefaultNode = "Door.Locked";
    private const string UnlockedDefaultNode = "Door.Unlocked";
    private const string UnlockChoiceDefaultNode = "Door.UnlockChoice";
    private const string JustUnlockedDefaultNode = "Door.JustUnlocked";

    [Header("Door Specific nodes")]
    public string LockedNode = LockedDefaultNode;
    public string UnlockedNode = UnlockedDefaultNode;
    public string JustUnlockedNode = JustUnlockedDefaultNode;
    public string UnlockChoiceNode = UnlockChoiceDefaultNode;
}
