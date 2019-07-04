using UnityEngine;

/// <summary>
/// Data container for the four directions
/// </summary>
public abstract class CardinalData<T> : ScriptableObject
{
    public T Top;
    public T Bottom;
    public T Right;
    public T Left;

    public T Get(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top: return Top;
            case Direction.Bottom: return Bottom;
            case Direction.Right: return Right;
            case Direction.Left: return Left;
        }
        return default;
    }
}
