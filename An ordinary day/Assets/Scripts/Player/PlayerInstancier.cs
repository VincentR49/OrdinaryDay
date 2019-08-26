using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton to instanciate the player at the given spawn point
/// </summary>
public class PlayerInstancier : Singleton<PlayerInstancier>
{
    private static GameObject _playerPrefab = _playerPrefab = Utils.GetPrefab(PathConstants.PlayerPrefab);

    public static void InstanciatePlayer(SpawnData spawn)
    {
        // Search for the player
        var player = InstanciateIfNeeded(_playerPrefab);
        Spawner.Spawn(player, spawn,
            new List<MonoBehaviour>
            {
                player.GetComponent<PlayerController>()
            });
    }


    public static GameObject InstanciateIfNeeded(GameObject playerPrefab)
    {
        var player = GameObject.FindWithTag(playerPrefab.tag);
        if (player == null) // doesnt exit, we instanciate him
            player = Instantiate(playerPrefab);
        return player;
    }
}
