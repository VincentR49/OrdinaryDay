using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// Manage the loading in the game
// Should be present from the begining of the game
public class LoadingHandler : MonoBehaviour
{
    [SerializeField]
    private SceneReference _loadingScene;

    public static LoadingHandler Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Several instances of loadinghandler are running");
            Destroy(gameObject);
            return;
        }
    }


    public void OpenLoadingScene()
    {
        StartCoroutine(OpenLoadingSceneRoutine());
    }


    public IEnumerator OpenLoadingSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(_loadingScene.Path);
    }
}
