using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// Manage the load / unload of scenes
// todo always fading
// todo not waiting for fade to disappear
public class SceneLoader : Singleton<SceneLoader>
{
    private const float FadeDuration = 0.1f;
    private const float LoadingTime = 2f; // todo debug
    private const string LoadingScene = "Loading";
    public static float LoadingProgress { private set; get; }
    private static bool _fade;
    private static bool _loadingScreen;
    private static string _sceneName;

    #region Load
    // todo Remove fade parameter?
    public static void LoadScene(string sceneName, bool fade, bool loadingScreen)
    {
        _fade = fade;
        _loadingScreen = loadingScreen;
        _sceneName = sceneName;
        Instance.StartLoading();
    }


    private IEnumerator FadeOut()
    {
        ScreenFader.Instance.FadeOut(FadeDuration);
        yield return new WaitForSeconds(FadeDuration);
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
        yield return StartCoroutine(Instance.LoadSceneRoutine(LoadingScene));
        yield return new WaitForSeconds(LoadingTime);
        yield return StartCoroutine(Instance.LoadSceneRoutine(sceneName));
    }


    private void StartLoadSceneRoutine(string sceneName)
    {
        StartCoroutine(Instance.LoadSceneRoutine(sceneName));
    }


    private IEnumerator LoadSceneRoutine(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        Debug.Log("Start to load scene: " + sceneName + ", " + loadMode);
        if (_fade)
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
        if (_fade)
            ScreenFader.Instance.FadeIn(FadeDuration);
    }
    #endregion
}
