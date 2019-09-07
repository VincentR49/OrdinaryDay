using UnityEngine;

/// <summary>
/// Holding data for a game item, that can be kept in an inventory
/// </summary>
[CreateAssetMenu(menuName ="Scriptables/Game Item")]
public class GameItemData : ScriptableObject
{
    public string Tag;
    public string Name;
    public string Description;
    public Sprite Sprite;
}
