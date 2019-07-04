using UnityEngine;

[CreateAssetMenu(menuName ="Scriptables/PNJData")]
public class PNJData : ScriptableObject
{
    [Header("Standard Information")]
    public string FirstName;
    public string LastName;
    public int Age;
    public Gender Gender;

    [Header("Sprites")]
    public CardinalSpriteData CardinalSprite;
    public CardinalAnimationData WalkingAnimation;

}
