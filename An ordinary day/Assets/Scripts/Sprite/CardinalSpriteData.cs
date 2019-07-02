using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CardinalSprite")]
public class CardinalSpriteData : ScriptableObject
{
    public Sprite Top;
    public Sprite Bottom;
    public Sprite Right;
    public Sprite Left;
    public bool FlipXForRightOrLeft;

    public Sprite GetSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top: return Top;
            case Direction.Bottom: return Bottom;
            case Direction.Right: return Right;
            case Direction.Left: return Left;
        }
        return null;
    }
}
