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


    #region Extensions Methods
    public static Node GetNodeByPosition(this List<Node> list, Node item)
    {
        return list.FirstOrDefault((x) => x.Position == item.Position);
    }
    #endregion

}
