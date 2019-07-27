using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawn to the given spawn Tag in the given Scene
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Tasks/Spawn")]
public class Spawn : Task
{
    public string SpawnPointTag;
    public SceneReference Scene;

    public bool IsInCurrentScene() => Scene.Path.Equals(SceneManager.GetActiveScene().path);
}