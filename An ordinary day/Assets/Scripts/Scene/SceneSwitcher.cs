using UnityEngine;
using UnityEngine.SceneManagement;

// Sitch to the given scene contained in the scene picker.
// todo add some parameters
public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private SceneReference _destinationScene = default;
    [SerializeField]
    private bool _withLoadingScene = default;
    [SerializeField]
    private bool _fade = default;

    public void Switch()
    {
        SceneLoader.LoadScene(_destinationScene.Path, _fade, _withLoadingScene);
    }
}
