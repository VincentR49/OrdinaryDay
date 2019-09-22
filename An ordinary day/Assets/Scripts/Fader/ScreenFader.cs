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
                var go = SingletonFactory.ScreenFader();
                // assignation is then done on the awake (a bit dirty)

                // actually not needed
                //_instance = go.GetComponent<ScreenFader>();
            }
            return _instance;
        }
    }

    new private void Awake()
    {
        base.Awake();
        Debug.Log("Screen fader awake");
        if (_instance == null)
        {
            Debug.Log("Screen fader instance assignation");
            _instance = this;
        }
        else
        {
            Debug.LogWarning("Several instances of ScreenFader are detected");
            Destroy(gameObject);
            return;
        }
    }
}
