using UnityEngine;

/// <summary>
/// Dialogue Data specific for GameItem
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Dialogue/Game Item")]
public class GameItemDialogueData : DialogueAgentData
{
    [Header("Game Item")]
    public GameItemData GameItem;

    [Header("Reward")]
    // optional if no reward is related to the object
    public TextAsset RewardDialogueFile;
    public string RewardNode;

    public void UpdateContent()
    {
        Tag = GameItem.Tag;
        DialoguePicture = GameItem.Sprite;
    }
}
