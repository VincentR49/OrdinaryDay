using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// Manage the load / unload of scenes
// todo always fading
// todo not waiting for fade to disappear
public class SceneLoader : Singleton<SceneLoader>
{
    private const float LoadingTime = 0f; // todo debug
    private const string LoadingScene = "Loading";
    public static float LoadingProgress { private set; get; }
    private static float _fadeDuration;
    private static bool _loadingScreen;
    private static string _sceneName;

    #region Load
    public static void LoadScene(string sceneName, float fadeDuration, bool loadingScreen, Action callAfterLoading = null)
    {
        if (GamePauser.IsPaused)
            GamePauser.Resume();
        _fadeDuration = fadeDuration;
        _loadingScreen = loadingScreen;
        _sceneName = sceneName;
        Instance.StartLoading(callAfterLoading);
    }


    private IEnumerator FadeOut()
    {
        ScreenFader.Instance.FadeOut(_fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
    }

    private void StartLoading(Action callAfterLoading = null)
    {
        if (_loadingScreen)
        {
            StartCoroutine(LoadSceneWithLoadingRoutine(_sceneName, callAfterLoading));
        }
        else
        {
            StartCoroutine(LoadSceneRoutine(_sceneName, callAfterLoading));
        }
    }


    private IEnumerator LoadSceneWithLoadingRoutine(string sceneName, Action callAfterLoading = null)
    {
        yield return StartCoroutine(LoadSceneRoutine(LoadingScene));
        yield return new WaitForSeconds(LoadingTime);
        yield return StartCoroutine(LoadSceneRoutine(sceneName, callAfterLoading));
    }


    private IEnumerator LoadSceneRoutine(string sceneName, Action callAfterLoading = null)
    {
        Debug.Log("Start to load scene: " + sceneName);
        yield return FadeOut();
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);
        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;
        while (!asyncScene.isDone)
        {
            // loading bar progress
            LoadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;
            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }
            yield return null;
        }
        Debug.Log("Scene loaded: " + sceneName);
        ScreenFader.Instance.FadeIn(_fadeDuration/2);
        if (callAfterLoading != null)
            callAfterLoading.Invoke();
    }
    #endregion
}
