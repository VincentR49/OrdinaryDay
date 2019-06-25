using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manage the load / unload of scenes
public class SceneLoader : Singleton<SceneLoader>
{
    private const string LoadingSceneTag = "Loading";
    // Loading Progress: private setter, public getter
    private float _loadingProgress;
    public float LoadingProgress => _loadingProgress;

    #region Load
    public void LoadScene(string sceneName, bool withLoading = false)
    {
        if (withLoading)
        {
            StartCoroutine(LoadSceneWithLoadingRoutine(sceneName));
        }
        else
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }
    }

    private IEnumerator LoadSceneWithLoadingRoutine(string sceneName)
    {
        // LoadSceneAsync() returns an AsyncOperation, 
        // so will only continue past this point when the Operation has finished
        yield return SceneManager.LoadSceneAsync(LoadingSceneTag);
        // as soon as we've finished loading the loading screen, start loading the game scene
        yield return StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        Debug.Log("Start to load scene: " + sceneName);
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;
        while (!asyncScene.isDone)
        {
            // loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;
            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }
            yield return null;
        }
        Debug.Log("Scene loaded: " + sceneName);
    }
    #endregion


    #region Unload
    public void UnloadScene(string sceneName)
    {
        var scene = Utils.IsPath(sceneName) ? 
            SceneManager.GetSceneByPath(sceneName) 
            : SceneManager.GetSceneByName(sceneName);
        if (scene != null)
        {
            Debug.Log("Unload Scene: " + sceneName);
            StartCoroutine(UnloadSceneRoutine(scene));
        }
    }


    private IEnumerator UnloadSceneRoutine(Scene scene)
    {
        Debug.Log("Start to unload scene: " + scene.name);
        yield return SceneManager.UnloadSceneAsync(scene.buildIndex);
        Debug.Log("Scene unloaded: " + scene.name);
    }
    #endregion
}
