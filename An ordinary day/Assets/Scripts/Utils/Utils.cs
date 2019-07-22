using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Utils
{
    public static bool IsPath(string str)
    {
        if (str == null)
            return false;
        return str.Contains("/") || str.Contains("\\");
    }

    public static float Distance(Vector2 u, Vector2 v)
        => Mathf.Sqrt(Mathf.Pow(u.x - v.x, 2) + Mathf.Pow(u.y - v.y, 2));


    public static Direction GetDirection(Vector2 v)
    {
        var absX = Mathf.Abs(v.x);
        var absY = Mathf.Abs(v.y);
        if (absX >= absY)
            return v.x > 0 ? Direction.West : Direction.East;
        else
            return v.y > 0 ? Direction.North : Direction.South;
    }

    public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);

    #region Extensions Methods
    public static Node GetNodeByPosition(this List<Node> list, Node item)
    {
        return list.FirstOrDefault((x) => x.Position == item.Position);
    }
    #endregion

}
