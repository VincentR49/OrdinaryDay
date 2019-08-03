using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spawner : MonoBehaviour
{
    private const float SpawnDuration = 0.25f;

    [SerializeField]
    private SpawnerList _spawnerList = default;
    [SerializeField]
    private SpawnData _spawnData;

    public delegate void SpawnFinishHandler(GameObject go);
    public event SpawnFinishHandler OnSpawnFinished;

    public SpawnData GetSpawnData() => _spawnData;

    private void Awake()
    {
        if (!_spawnData.IsInCurrentScene())
            Debug.LogError("Wrong scene for this spawn data, shoudlnt happen !");
        _spawnerList.Add(this);
    }


    private void OnDestroy()
    {
        _spawnerList.Remove(this);
    }


    public void Spawn(GameObject go, List<MonoBehaviour> disableDuringSpawn = null, Action executeAfterSpawn = null)
    {
        var spriteDirectioner = go.GetComponent<SpriteDirectioner>();
        StartCoroutine(SpawnCoroutine(go, spriteDirectioner, disableDuringSpawn));
    }


    private IEnumerator SpawnCoroutine(GameObject go, SpriteDirectioner spriteDirectioner = null, List<MonoBehaviour> disableDuringSpawn = null, Action executeAfterSpawn = null)
    {
        Debug.Log("Start spawn coroutine on " + go.name);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = false;
        }
        go.transform.position = gameObject.transform.position;
        if (spriteDirectioner != null)
            spriteDirectioner.SetSprite(_spawnData.SpawnDirection);
        yield return new WaitForSeconds(SpawnDuration);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = true;
        }
        Debug.Log("Finish spawn on " + go.name);
        OnSpawnFinished?.Invoke(go);
        if (executeAfterSpawn != null)
            executeAfterSpawn.Invoke();
    }
}
