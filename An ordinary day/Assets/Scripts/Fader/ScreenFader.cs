using UnityEngine;

// Singleton class heriting from fader
// TODO make a screen Fader Factory !
public class ScreenFader : Fader
{
    public static ScreenFader Instance;

    new private void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Several instances of ScreenFader are detected");
            Destroy(gameObject);
            return;
        }
    }
}
