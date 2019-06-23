using UnityEngine;

// Sitch to the given scene contained in the scene picker.
// todo add some parameters
public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private ScenePicker _destinationScene;

    public void Switch()
    {
        SceneLoaderAsync.Instance.LoadScene(_destinationScene.ScenePath);
    }
}
