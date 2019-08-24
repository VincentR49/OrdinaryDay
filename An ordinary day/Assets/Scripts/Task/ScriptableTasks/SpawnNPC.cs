using UnityEngine;

/// <summary>
/// Spawn the executor object to the spawn Tag in the given Scene
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Tasks/Spawn")]
public class SpawnNPC : Task
{
    public NPCData NPC;
    public SpawnData SpawnData;

    public bool IsInCurrentScene() => SpawnData.IsInCurrentScene();
}