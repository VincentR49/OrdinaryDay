using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Dialogue Agent")]
public class DialogueAgentData : ScriptableObject
{
    public string Tag;
    public string DialogueDisplayName;
    public string StoryNode;
    public TextAsset YarnDialogue;
    public Sprite DialoguePicture;
}
