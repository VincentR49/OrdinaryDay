using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spawner : Singleton<Spawner>
{
    private const float DefaultSpawnDuration = 0.25f;
    private const float EPSILON = 0.0001f;
    public delegate void SpawnFinishHandler(GameObject go);
    public event SpawnFinishHandler OnSpawnFinished;


    public static void Spawn(GameObject go, SpawnData spawn, List<MonoBehaviour> disableDuringSpawn = null, Action executeAfterSpawn = null, float spawnDuration = -1f)
    {
        if (!spawn.IsInCurrentScene())
        {
            Debug.LogError("Spawn should be in current scene: " + spawn.name);
            return;
        }
        Instance.StartCoroutine(SpawnCoroutine(go, spawn.Position, spawn.Direction, disableDuringSpawn, executeAfterSpawn, spawnDuration));
    }


    public static void Spawn(GameObject go, Vector2 position, Direction direction, List<MonoBehaviour> disableDuringSpawn = null, Action executeAfterSpawn = null, float spawnDuration = -1f)
    {
        Instance.StartCoroutine(SpawnCoroutine(go, position, direction, disableDuringSpawn, executeAfterSpawn, spawnDuration));
    }


    private static IEnumerator SpawnCoroutine(GameObject go, Vector2 position, Direction direction, List<MonoBehaviour> disableDuringSpawn = null, Action executeAfterSpawn = null, float spawnDuration = -1f)
    {
        Debug.Log("Start spawn coroutine on " + go.name);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = false;
        }
        go.transform.position = position;
        var spriteDirectioner = go.GetComponent<SpriteDirectioner>();
        if (spriteDirectioner != null)
            spriteDirectioner.SetSprite(direction);
        yield return new WaitForSeconds(Math.Abs(spawnDuration - 1f) < EPSILON ? DefaultSpawnDuration : spawnDuration);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = true;
        }
        Debug.Log("Finish spawn on " + go.name);
        Instance.OnSpawnFinished?.Invoke(go);
        if (executeAfterSpawn != null)
            executeAfterSpawn.Invoke();
    }
}
