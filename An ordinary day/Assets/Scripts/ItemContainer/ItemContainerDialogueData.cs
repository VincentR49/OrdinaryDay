using UnityEngine;

/// <summary>
/// Data holder of Dialogue Information for Yarn Plugin
/// Use this one for item container kind of dialogue.
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Dialogue/Container")]
public class ItemContainerDialogueData : DialogueAgentData
{
    private const string EmptyDefaultNode = "Container.Empty";
    private const string NotEmptyDefaultNode = "Container.NotEmpty";
    private const string JustTookItemDefaultNode = "Container.JustTookItem";

    [Header("Container Specific nodes")]
    public string EmptyNode = EmptyDefaultNode;
    public string NotEmptyNode = NotEmptyDefaultNode;
    public string JustTookItemNode = JustTookItemDefaultNode;
}
