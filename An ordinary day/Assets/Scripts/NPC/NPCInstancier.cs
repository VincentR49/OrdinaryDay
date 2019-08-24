using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Singleton to instanciate NPC
/// </summary>
public class NPCInstancier : Singleton<NPCInstancier>
{
    private static NPCController _prefab =
        Utils.GetPrefab(PathConstants.NPCPrefab).GetComponent<NPCController>();


    public static NPCController InstanciateNPC(NPCData npcData, SpawnData spawnData, Action executeAfterSpawn = null)
    {
        if (!spawnData.IsInCurrentScene())
        {
            Debug.LogError("Spawn should be in different scene.");
            return null;
        }
        return InstanciateNPC(npcData, spawnData.Position, spawnData.Direction, executeAfterSpawn);
    }

    
    public static NPCController InstanciateNPC(NPCData npcData, Vector2 position, Direction direction, Action executeAfterSpawn = null)
    {
        var npc = Instance.InstanciateIfNeeded(npcData);
        Spawner.Spawn(npc.gameObject, position, direction, new List<MonoBehaviour>
        {
            npc.GetComponent<ScheduleHandler>()
        }, executeAfterSpawn, 0f);
        return npc;
    }


    private NPCController InstanciateIfNeeded(NPCData npcData)
    {
        var npc = NPCController.Get(npcData);
        if (npc == null) // if not in current scene, make it spawn
            npc = Instantiate(_prefab);
        npc.Init(npcData);
        return npc;
    }
}
