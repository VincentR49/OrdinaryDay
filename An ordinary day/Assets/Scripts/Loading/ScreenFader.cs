using UnityEngine;
using UnityEngine.Events;

public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    private Fader _fader;

    public static UnityEvent FadeInFinished => Instance._fader.FadeInFinished;
    public static UnityEvent FadeOutFinished => Instance._fader.FadeOutFinished;

    public static ScreenFader Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Several instances of ScreenFader are running");
            Destroy(gameObject);
            return;
        }
    }


    public static void FadeIn() => Instance._fader.FadeIn();
    public static void FadeOut() => Instance._fader.FadeOut();
}
