using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

/// <summary>
/// Need to be clean later
/// </summary>
public static class Utils
{
    public static List<T> FindAssetsByType<T>(string folder = null) where T : UnityEngine.Object
    {
        return FindAssetsByType<T>(folder == null ? null : new string[] { folder });
    }

    public static List<T> FindAssetsByType<T>(string[] searchInFolders = null) where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)), searchInFolders);
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }

    public static bool IsPath(string str)
    {
        if (str == null)
            return false;
        return str.Contains("/") || str.Contains("\\");
    }

    public static float Distance(Vector2 u, Vector2 v)
        => Mathf.Sqrt(Mathf.Pow(u.x - v.x, 2) + Mathf.Pow(u.y - v.y, 2));


    public static GameObject GetPrefab(string path)
    {
        var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        if (prefab == null)
        {
            Debug.LogError("Couldnt find prefab at " + path);
        }
        return prefab;
    }


    public static Direction GetDirection(Vector2 v)
    {
        var absX = Mathf.Abs(v.x);
        var absY = Mathf.Abs(v.y);
        if (absX >= absY)
            return v.x > 0 ? Direction.East : Direction.West;
        else
            return v.y > 0 ? Direction.North : Direction.South;
    }

    public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);

    public static DateTime GetDateTime(DayDate dayDate, DayTime dayTime)
        =>  new DateTime(dayDate.Year, dayDate.Month, dayDate.Day, dayTime.Hour, dayTime.Min, dayTime.Sec);


    #region Extensions Methods
    public static Node GetNodeByPosition(this List<Node> list, Node item)
    {
        return list.FirstOrDefault((x) => x.Position == item.Position);
    }
    #endregion

}
