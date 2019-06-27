using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SceneReference _firstGameScene = default;
   
    private static bool _alreadyExists;
    public static bool IsPaused => Math.Abs(Time.timeScale) < 0.001f;

    private void Awake()
    {
        if (!_alreadyExists)
        {
            _alreadyExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    public void StartNewGame()
    {
        Debug.Log("[GameManager] Start NewGame");
        LoadFirstScene();
    }


    public void LoadGame()
    {
        // todo
        Debug.LogError("TODO: Load Game");
    }


    public void QuitGame()
    {
        Debug.Log("[GameManager] Quit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }


    public void Pause()
    {
        Time.timeScale = 0;
    }


    public void Resume()
    {
        Time.timeScale = 1f;
    }


    private void LoadFirstScene()
    {
        // todo change this
        SceneLoader.LoadScene(_firstGameScene.Path, 0.5f, true);
    }
}