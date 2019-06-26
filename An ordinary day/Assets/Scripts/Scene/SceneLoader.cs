using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// Manage the load / unload of scenes
// todo always fading
// todo not waiting for fade to disappear
public class SceneLoader : Singleton<SceneLoader>
{
    private const float LoadingTime = 2f; // todo debug
    public static float LoadingProgress { private set; get; }
    public static event LoadingFinished OnLoadingFinished;
    public delegate void LoadingFinished();

    private static bool _fade;
    private static bool _loadingScreen;
    private static bool _busy;
    private static string _sceneName;
    private static UnityAction _afterFadeOutAction;

    #region Load
    public static void LoadScene(string sceneName, bool fade, bool loadingScreen)
    {
        if (_busy)
        {
            Debug.LogError("Already loading a scene");
            return;
        }
        _busy = true;
        _fade = fade;
        _loadingScreen = loadingScreen;
        _sceneName = sceneName;
        if (_fade)
        {
            Instance.StartFadeOut(Instance.StartLoading);
        }
        else
        {
            Instance.StartLoading();
        }
    }

    private void StartFadeOut(UnityAction afterFadeAction)
    {
        _afterFadeOutAction = afterFadeAction;
        ScreenFader.FadeOutFinished.AddListener(Instance.OnFadeOutFinished);
        ScreenFader.FadeOut();
    }


    private void OnFadeOutFinished()
    {
        Debug.Log("OnFadeOutFinished");
        _afterFadeOutAction.Invoke();
        ScreenFader.FadeOutFinished.RemoveListener(Instance.OnFadeOutFinished);
    }


    private void StartLoading()
    {
        if (_loadingScreen)
        {
            StartCoroutine(Instance.LoadSceneWithLoadingRoutine(_sceneName));
        }
        else
        {
            StartCoroutine(Instance.LoadSceneRoutine(_sceneName));
        }
    }


    private IEnumerator LoadSceneWithLoadingRoutine(string sceneName)
    {
        // Loading screen
        if (_fade)
        {
            ScreenFader.FadeIn();
        }
        yield return LoadingHandler.Instance.OpenLoadingSceneRoutine();
        yield return new WaitForSeconds(LoadingTime);
        if (_fade)
        {
            StartFadeOut(() => StartLoadSceneRoutine(sceneName));
        }
        else
        {
            yield return StartCoroutine(Instance.LoadSceneRoutine(sceneName));
        }
    }


    private void StartLoadSceneRoutine(string sceneName)
    {
        StartCoroutine(Instance.LoadSceneRoutine(sceneName));
    }


    private IEnumerator LoadSceneRoutine(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        Debug.Log("Start to load scene: " + sceneName + ", " + loadMode);
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
        if (_fade)
        {
            ScreenFader.FadeIn();
        }
        _busy = false;
    }
    #endregion
}
