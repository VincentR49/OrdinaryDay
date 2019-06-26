using UnityEngine;
using UnityEngine.SceneManagement;

// Sitch to the given scene contained in the scene picker.
// todo add some parameters
public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private SceneReference _destinationScene;
    [SerializeField]
    private bool _withLoadingScene;
    [SerializeField]
    private bool _fade;

    public void Switch()
    {
        SceneLoader.LoadScene(_destinationScene.Path, _fade, _withLoadingScene);
    }
}
