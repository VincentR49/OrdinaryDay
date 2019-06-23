using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ScenePicker _firstGameScenePicker;
   
    private static GameManager _instance;
    public static bool IsPaused => Math.Abs(Time.timeScale) < 0.001f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    public void StartNewGame()
    {
        Debug.Log("[GameManager] Start NewGame");
        SceneManager.LoadScene(_firstGameScenePicker.ScenePath);
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

    public void OnNewTimeLoopStarted()
    {
        Debug.Log("[GameManager] Reset Game after loop");
        SceneManager.LoadScene(_firstGameScenePicker.ScenePath);
    }


    public void Pause()
    {
        Time.timeScale = 0;
    }


    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
