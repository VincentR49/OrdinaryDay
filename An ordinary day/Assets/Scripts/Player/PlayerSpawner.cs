using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawn the player on Start at the given spawn point
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField]
    private SpawnDataVariable _playerNextSpawn;
    [SerializeField]
    private SpawnerList _spawnerList;
    [SerializeField]
    private GameObject _playerPrefab;

    private void Start()
    {
        var spawnPoint = _spawnerList.GetSpawner(_playerNextSpawn.Value);
        if (spawnPoint)
        {
            // Search for the player
            var player = InstanciateIfNeeded();
            spawnPoint.Spawn(player,
                new List<MonoBehaviour>
                { 
                    player.GetComponent<PlayerController>()
                });
            _playerNextSpawn.Value = null;
        }
    }


    private GameObject InstanciateIfNeeded()
    {
        var player = GameObject.FindWithTag(PlayerTag);
        if (player == null) // doesnt exit, we instanciate him
            player = Instantiate(_playerPrefab);
        return player;
    }
}
