using UnityEngine;
using System;

/// <summary>
/// Define a AStar algoritm Node
/// </summary>
public class Node : IComparable<Node>
{
    public Node Parent { get; private set; }
    public float Cost { get; set; }
    public float Heuristic { get; set; }
    public Vector2Int Position { get; private set; }

    public Node(Vector2Int position, Node parent, float cost = 0f, float heuristic = 0f)
    {
        Parent = parent;
        Position = position;
        Cost = cost;
        Heuristic = heuristic;
    }

    public float GetDistanceFrom(Node other)
    {
        return (float) (Math.Pow(Position.x - other.Position.x, 2) 
                      + Math.Pow(Position.y - other.Position.y, 2));
    }

    public int CompareTo(Node other)
    {
        if (Heuristic < other.Heuristic)      return -1;
        else if (Heuristic > other.Heuristic) return 1;
        return 0;
    }


    public override string ToString()
    {
        //return string.Format("[{0}|({1},{2})]", Position.ToString(), Cost, Heuristic);
        return Position.ToString();
    }
}


