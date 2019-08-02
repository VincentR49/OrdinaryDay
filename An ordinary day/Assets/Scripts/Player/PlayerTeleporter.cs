using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Teleport the player to the given position (on the same scene or another scene)
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerTeleporter : MonoBehaviour
{
    private const float Fade = 0.25f;
    private const string PlayerTag = "Player";
    [SerializeField]
    private SpawnData _targetSpawn;
    [SerializeField]
    private SpawnerList _spawnerList = default;
    [SerializeField]
    private SpawnDataVariable _playerNextSpawnData;

    protected bool _isTeleporting;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isTeleporting || !IsPlayer(collision))
            return;
        TeleportPlayer(collision.gameObject);
    }


    protected void TeleportPlayer(GameObject player)
    {
        Debug.Log("Teleport to scene: " + _targetSpawn.Scene.Path + " at spawnPoint " + _targetSpawn.name);
        _isTeleporting = true;
        if (_targetSpawn.IsInCurrentScene())
        {
            // teleport inside the scene without loading
            var spawnPoint = _spawnerList.GetSpawner(_targetSpawn);
            if (spawnPoint) // teleport the player
            {
                player.GetComponent<SpriteAnimator>().StopCurrentAnimation();
                spawnPoint.Spawn(player, new List<MonoBehaviour>
                {
                    player.GetComponent<PlayerController>()
                });
                _isTeleporting = false;
            }
        }
        else // go to different scene
        {
            _playerNextSpawnData.Value = _targetSpawn;
            SceneLoader.LoadScene(_targetSpawn.Scene.Path, Fade, false);
        }
    }


    protected bool IsPlayer(Collider2D collision) => collision.tag.Equals(PlayerTag);
}
