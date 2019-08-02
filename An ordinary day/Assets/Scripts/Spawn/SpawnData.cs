using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scriptables/SpawnData")]
public class SpawnData : ScriptableObject
{
    public Direction SpawnDirection;
    public SceneReference Scene;

    public bool IsInCurrentScene() => SceneManager.GetActiveScene().path.Equals(Scene.Path);
}
