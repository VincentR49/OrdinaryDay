using System;
using UnityEngine;


public static class GamePauser
{
    public static bool IsPaused => Math.Abs(Time.timeScale) < 0.001f;

    public static void Pause()
    {
        Debug.Log("Pause Game");
        Time.timeScale = 0f;
    }


    public static void Resume()
    {
        Debug.Log("Resume Game");
        Time.timeScale = 1f;
    }
}
