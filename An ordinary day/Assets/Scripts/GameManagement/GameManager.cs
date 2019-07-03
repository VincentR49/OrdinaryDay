using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    private void Update()
    {

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


    private void LoadFirstScene()
    {
        SceneLoader.LoadScene(_firstGameScene.Path, 0.5f, true);
        SceneManager.sceneLoaded += OnFirstLevelLoaded;
    }


    private void PauseGame()
    {
        Time.timeScale = 0f;
    }


    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }


    private void OnFirstLevelLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.path.Equals(_firstGameScene.Path))
        {
            SceneManager.sceneLoaded -= OnFirstLevelLoaded;
            TimeManager.Init();
            //PauseGame();
            // TODO press key to start
        }
    }
}