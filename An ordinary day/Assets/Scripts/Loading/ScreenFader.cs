using UnityEngine;

// Singleton class heriting from fader
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
            Debug.LogError("Several instances of ScreenFader are running");
            Destroy(gameObject);
            return;
        }
    }
}
