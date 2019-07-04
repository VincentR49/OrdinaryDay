using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
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

    private static Action _callAfterLoading;

    #region Load
    public static void LoadScene(string sceneName, float fadeDuration, bool loadingScreen, Action callAfterLoading = null)
    {
        _fadeDuration = fadeDuration;
        _loadingScreen = loadingScreen;
        _sceneName = sceneName;
        _callAfterLoading = callAfterLoading;
        Instance.StartLoading();
    }


    private IEnumerator FadeOut()
    {
        ScreenFader.Instance.FadeOut(_fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
    }

    private void StartLoading()
    {
        if (_loadingScreen)
        {
            StartCoroutine(LoadSceneWithLoadingRoutine(_sceneName));
        }
        else
        {
            StartCoroutine(LoadSceneRoutine(_sceneName));
        }
    }


    private IEnumerator LoadSceneWithLoadingRoutine(string sceneName)
    {
        yield return StartCoroutine(LoadSceneRoutine(LoadingScene));
        yield return new WaitForSeconds(LoadingTime);
        yield return StartCoroutine(LoadSceneRoutine(sceneName));
    }


    private void StartLoadSceneRoutine(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }


    private IEnumerator LoadSceneRoutine(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        Debug.Log("Start to load scene: " + sceneName + ", " + loadMode);
        yield return FadeOut();
        var asyncScene = SceneManager.LoadSceneAsync(sceneName, loadMode);
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
        if (_callAfterLoading != null)
            ScreenFader.Instance.FadeInFinished.AddListener(OnLoadingFinished);
    }
    #endregion

    private void OnLoadingFinished()
    {
        Debug.Log("Execute OnLoadingFinished Action");
        ScreenFader.Instance.FadeInFinished.RemoveListener(OnLoadingFinished);
        _callAfterLoading.Invoke();
    }
}
