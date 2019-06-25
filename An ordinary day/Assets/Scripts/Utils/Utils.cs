using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool IsPath(string str)
    {
        if (str == null)
            return false;
        return str.Contains("/") || str.Contains("\\");
    }
}
