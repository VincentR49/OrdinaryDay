using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string _mainSceneTag = "MainScene";
     
    [SerializeField]
    private TimeLooper _timeLooper;

    private static GameManager _instance;

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
        DontDestroyOnLoad(this);
    }


    private void Start()
    {
        InitGame();
    }


    public void InitGame()
    {
        Debug.Log("[GameManager] Init Game");
        _timeLooper.Init();
    }


    public void ResetGame()
    {
        Debug.Log("[GameManager] Reset Game after loop");
        SceneManager.LoadScene(_mainSceneTag);
    }
}
