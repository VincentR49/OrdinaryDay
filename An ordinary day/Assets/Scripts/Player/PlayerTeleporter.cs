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
    private SceneReference _sceneDestination = default;
    [SerializeField]
    private string _spawnDestinationTag = default;
    [SerializeField]
    private SpawnPointList _spawnList = default;
    [SerializeField]
    private StringData _playerSpawnPointTag;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.tag.Equals(PlayerTag))
            return;
        var player = collision.gameObject;
        Debug.Log("Teleport to scene: " + _sceneDestination.Path + " at spawnPoint " + _spawnDestinationTag);
        if (_sceneDestination.Path.Equals(SceneManager.GetActiveScene().path))
        {
            // teleport inside the scene without loading
            var spawnPoint = _spawnList.GetSpawnPoint(_spawnDestinationTag);
            if (spawnPoint) // teleport the player
            {
                var spriteDirectioner = player.GetComponent<SpriteDirectioner>();
                player.GetComponent<SpriteAnimator>().StopCurrentAnimation();
                var componentsToDisable = new List<MonoBehaviour>
                {
                    player.GetComponent<PlayerController>()
                };
                spawnPoint.Spawn(player, spriteDirectioner, componentsToDisable);
            }
        }
        else // go to different scene
        {
            _playerSpawnPointTag.Value = _spawnDestinationTag;
            SceneLoader.LoadScene(_sceneDestination.Path, Fade, false);
        }
    }
}
