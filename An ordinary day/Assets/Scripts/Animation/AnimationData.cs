using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/AnimationData")]
public class AnimationData : ScriptableObject
{
    public bool FlipX = false;
    public bool Loop = false;
    public bool ReturnToFirstSpriteWhenFinished = true;
    public Sprite[] Sprites;
}
