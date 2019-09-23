using UnityEngine;
using UnityEngine.Events;

// Singleton class heriting from fader
[Prefab("Assets/Prefabs/UIs/ScreenFader.prefab")]
public class ScreenFader : PrefabSingleton<ScreenFader>
{
    [SerializeField]
    private Fader _fader;

    public static void FadeIn() => Instance._fader.FadeIn();
    public static void FadeIn(float duration) => Instance._fader.FadeIn(duration);
    public static void FadeOut() => Instance._fader.FadeOut();
    public static void FadeOut(float duration) => Instance._fader.FadeOut(duration);
    public static UnityEvent FadeInFinished => Instance._fader.FadeInFinished;
    public static UnityEvent FadeOutFinished => Instance._fader.FadeOutFinished;
}
