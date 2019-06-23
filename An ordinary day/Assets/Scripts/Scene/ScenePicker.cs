using UnityEngine;

public class ScenePicker : MonoBehaviour
{
    [SerializeField]
    private string _scenePath;
    public string ScenePath => _scenePath;
}
