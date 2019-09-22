using UnityEngine;

// Singleton class heriting from fader
public class ScreenFader : Fader
{
    private static ScreenFader _instance;

    public static ScreenFader Instance
    {
        get
        {
            if (_instance == null)
            {
                SingletonFactory.ScreenFader();
            }
            return _instance;
        }
    }

    new private void Awake()
    {
        base.Awake();
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("Several instances of ScreenFader are detected");
            DestroyImmediate(gameObject);
            return;
        }
    }
}
