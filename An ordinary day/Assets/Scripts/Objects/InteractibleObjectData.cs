using UnityEngine;

/// <summary>
/// Data holder for interactive objects
/// </summary>
[CreateAssetMenu(menuName ="Scriptables/Interactive Object")]
public class InteractibleObjectData : ScriptableObject
{
    public string Tag;

    [Header("Dialogue")]
    public TextAsset YarnDialogue;
    public string StartNodeStory;
    public Sprite DialoguePicture;
}
