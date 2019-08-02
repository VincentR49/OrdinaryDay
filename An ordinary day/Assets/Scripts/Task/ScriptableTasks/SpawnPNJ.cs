using UnityEngine;

/// <summary>
/// Spawn the executor object to the spawn Tag in the given Scene
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Tasks/Spawn")]
public class SpawnPNJ : Task
{
    public PNJData PNJ;
    public SpawnData SpawnData;

    public bool IsInCurrentScene() => SpawnData.IsInCurrentScene();
}