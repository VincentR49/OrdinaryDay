using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SceneReference _firstGameScene = default;
   
    private static bool _alreadyExists;


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
        if (GamePauser.IsPaused && Input.anyKeyDown)
        {
            GamePauser.Resume();
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

    #region Load first level

    // TODO move to game loader ?
    private void LoadFirstScene()
    {
        SceneLoader.LoadScene(_firstGameScene.Path, 0.5f, true);
        SceneManager.sceneLoaded += OnFirstLevelLoaded;
    }


    private void OnFirstLevelLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.path.Equals(_firstGameScene.Path))
        {
            SceneManager.sceneLoaded -= OnFirstLevelLoaded;
            ScreenFader.Instance.FadeInFinished.AddListener(OnFirstLevelFadeInFinished);
        }
    }


    private void OnFirstLevelFadeInFinished()
    {
        ScreenFader.Instance.FadeInFinished.RemoveListener(OnFirstLevelFadeInFinished);
        InitGame();
    }

    private void InitGame()
    {
        TimeManager.Init();
        GamePauser.Pause();
    }

    #endregion
}