﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable containing the Player informations
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string FirstName;
    public string LastName;

    [Header("Sprites")]
    public CardinalSpriteData CardinalSprite;
    public CardinalAnimationData WalkingAnimation;

    [Header("Dialogue")]
    public string DialogueTag;
    public Sprite DialoguePicture;
}
