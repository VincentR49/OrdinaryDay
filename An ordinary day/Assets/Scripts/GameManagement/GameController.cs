using System;
using UnityEngine;

public class GameController : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GamePauser.IsPaused)
                GamePauser.Resume();
            else
                GamePauser.Pause();
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
        SceneLoader.LoadScene(_firstGameScene.Path, 0.5f, true, new Action(InitGame));
    }

    private void InitGame()
    {
        Debug.Log("InitGame");
        TimeManager.Init();
    }

    #endregion
}