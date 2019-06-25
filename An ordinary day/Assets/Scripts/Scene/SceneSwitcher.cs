using UnityEngine;

// Sitch to the given scene contained in the scene picker.
// todo add some parameters
public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private SceneReference _destinationScene;
    [SerializeField]
    private bool _showLoading;

    public void Switch()
    {
        SceneLoader.Instance.LoadScene(_destinationScene.Path, _showLoading);
    }
}
