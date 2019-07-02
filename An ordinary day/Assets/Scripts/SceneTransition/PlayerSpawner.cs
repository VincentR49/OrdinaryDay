using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField]
    private StringData _playerSpawnPointTag;
    [SerializeField]
    private SpawnPointList _spawnPointList;
    [SerializeField]
    private GameObject _playerPrefab;

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("On level load");
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
                spawnPoint.Spawn(player, player.GetComponent<SpriteDirectioner>());
                _playerSpawnPointTag.Value = "";
            }

        }
    }
}
