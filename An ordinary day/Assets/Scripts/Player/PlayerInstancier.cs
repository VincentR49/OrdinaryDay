using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Instanciate the player on Start at the last recorded spawn point
/// </summary>
public class PlayerInstancier : MonoBehaviour
{
    [SerializeField]
    private RuntimeSpawnData _playerNextSpawn;
    [SerializeField]
    private GameObject _playerPrefab;

    private string PlayerTag => _playerPrefab.tag;

    private void Start()
    {
        if (_playerNextSpawn.Value != null && _playerNextSpawn.Value.IsInCurrentScene())
        {
            // Search for the player
            var player = InstanciateIfNeeded();
            Spawner.Spawn(player, _playerNextSpawn.Value,
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
