using UnityEngine;
using System;

// Sitch to the given scene contained in the scene picker.
// todo add some parameters
public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    protected SceneReference _destinationScene = default;
    [SerializeField]
    private bool _withLoadingScene = default;
    [SerializeField]
    private float _fade = 0.5f;

    public void Switch()
    {
        Switch(null);
    }


    public void Switch(Action afterLoadingCallback)
    {
        SceneLoader.LoadScene(_destinationScene.Path, _fade, _withLoadingScene, afterLoadingCallback);
    }
}
