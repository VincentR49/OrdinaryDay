using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInstancier : Singleton<PlayerInstancier>
{
    private static GameObject PlayerPrefab
    {
        get
        {
            if (_playerPrefab == null)
            {
                _playerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(PathConstants.PlayerPrefab, typeof(GameObject));
                if (_playerPrefab == null)
                {
                    Debug.LogError("Couldnt find PlayerPrefab at " + PathConstants.PlayerPrefab);
                }
            }
            return _playerPrefab;
        }
    }

    private static GameObject _playerPrefab;

    public static void InstanciatePlayer(SpawnData spawn)
    {
        // Search for the player
        var player = InstanciateIfNeeded(PlayerPrefab);
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
