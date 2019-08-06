using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spawner : Singleton<Spawner>
{
    private const float SpawnDuration = 0.25f;

    public delegate void SpawnFinishHandler(GameObject go);
    public event SpawnFinishHandler OnSpawnFinished;


    public static void Spawn(GameObject go, SpawnData spawn, List<MonoBehaviour> disableDuringSpawn = null, Action executeAfterSpawn = null)
    {
        if (!spawn.IsInCurrentScene())
        {
            Debug.LogError("Spawn should be in current scene: " + spawn.name);
            return;
        }
        Instance.StartCoroutine(SpawnCoroutine(go, spawn, disableDuringSpawn, executeAfterSpawn));
    }


    private static IEnumerator SpawnCoroutine(GameObject go, SpawnData spawn, List<MonoBehaviour> disableDuringSpawn = null, Action executeAfterSpawn = null)
    {
        Debug.Log("Start spawn coroutine on " + go.name);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = false;
        }
        go.transform.position = spawn.Position;
        var spriteDirectioner = go.GetComponent<SpriteDirectioner>();
        if (spriteDirectioner != null)
            spriteDirectioner.SetSprite(spawn.Direction);
        yield return new WaitForSeconds(SpawnDuration);
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
