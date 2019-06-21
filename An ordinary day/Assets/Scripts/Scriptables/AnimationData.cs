using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AnimationData")]
public class AnimationData : ScriptableObject
{
    public bool FlipX = false;
    public bool Loop = false;
    public bool ReturnToFirstSpriteWhenFinished = true;
    public Sprite[] Sprites;
}
