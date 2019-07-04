using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuSwitcher : MonoBehaviour
{
    private const string GameMenuSceneName = "BootScene";

    public void SwitchToGameMenu()
    {
        Debug.Log("Switch to game menu");
        TimeManager.StopTime();
        SceneLoader.LoadScene(GameMenuSceneName, 0.5f, false);
    }
}
