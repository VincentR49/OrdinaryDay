using UnityEngine;
using System.Collections.Generic;

// Teleport the player to the given position (on the same scene or another scene)
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerTeleporter : MonoBehaviour
{
    private const float Fade = 0.25f;
    
    [SerializeField]
    private SpawnData _targetSpawn;
    [SerializeField]
    private GameObject _playerPrefab;

    private string PlayerTag => _playerPrefab.tag;
    protected bool _isTeleporting;


    protected bool IsPlayer(Collider2D collision) => collision.tag.Equals(PlayerTag);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isTeleporting || !IsPlayer(collision))
            return;
        TeleportPlayer(collision.gameObject);
    }


    protected void TeleportPlayer(GameObject player)
    {
        Debug.Log("Teleport to scene: " + _targetSpawn.ScenePath + " at spawnPoint " + _targetSpawn.name);
        _isTeleporting = true;
        if (_targetSpawn.IsInCurrentScene())
        {
            // teleport inside the scene without loading
            player.GetComponent<SpriteAnimator>().StopCurrentAnimation();
            Spawner.Spawn(player, _targetSpawn, new List<MonoBehaviour>
            {
                player.GetComponent<PlayerController>()
            }, FinishTeleport);
        }
        else // go to different scene
        {
            SceneLoader.LoadScene(_targetSpawn.ScenePath, Fade, false,
                () => PlayerInstancier.InstanciatePlayer(_targetSpawn));
        }
    }


    private void FinishTeleport()
    {
        _isTeleporting = false;
    }
}
