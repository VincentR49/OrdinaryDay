using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField]
    private StringData _playerSpawnPointTag;
    [SerializeField]
    private SpawnPointList _spawnPointList;
    [SerializeField]
    private GameObject _playerPrefab;

    private void Start()
    {
        var spawnTag = _playerSpawnPointTag.Value;
        if (!string.IsNullOrEmpty(spawnTag))
        {
            var spawnPoint = _spawnPointList.GetSpawnPoint(spawnTag);
            if (spawnPoint)
            {
                // Search for the player
                var player = GameObject.FindWithTag(PlayerTag);
                if (player == null)
                {
                    player = Instantiate(_playerPrefab);
                }
                spawnPoint.Spawn(player, player.GetComponent<SpriteDirectioner>(),
                    new List<MonoBehaviour>
                    { 
                        player.GetComponent<PlayerController>()
                    });
                _playerSpawnPointTag.Value = "";
            }

        }
    }
}
