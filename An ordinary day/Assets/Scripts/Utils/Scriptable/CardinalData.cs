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
            case Direction.North: return Top;
            case Direction.South: return Bottom;
            case Direction.West: return Left;
            case Direction.East: return Right;
        }
        return default;
    }
}
