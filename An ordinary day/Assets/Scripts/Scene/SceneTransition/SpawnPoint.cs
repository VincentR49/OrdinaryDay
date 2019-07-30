using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    public delegate void SpawnFinishHandler(GameObject go);
    public event SpawnFinishHandler OnSpawnFinished;

    [SerializeField]
    private SpawnPointList _spawnPointList = default;
    [SerializeField]
    private string _tag = default;
    [SerializeField]
    private Direction _spawnDirection = default;

    private const float SpawnDuration = 0.25f;

    private void Awake()
    {
        _spawnPointList.Add(this);
    }


    private void OnDestroy()
    {
        _spawnPointList.Remove(this);
    }


    public string GetTag()
    {
        if (string.IsNullOrEmpty(_tag))
            return name;
        return _tag;
    }


    public void Spawn(GameObject go, List<MonoBehaviour> disableDuringSpawn = null)
    {
        var spriteDirectioner = go.GetComponent<SpriteDirectioner>();
        StartCoroutine(SpawnCoroutine(go, spriteDirectioner, disableDuringSpawn));
    }


    private IEnumerator SpawnCoroutine(GameObject go, SpriteDirectioner spriteDirectioner = null, List<MonoBehaviour> disableDuringSpawn = null)
    {
        Debug.Log("Start spawn coroutine on " + go.name);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = false;
        }
        go.transform.position = gameObject.transform.position;
        if (spriteDirectioner != null)
            spriteDirectioner.SetSprite(_spawnDirection);
        yield return new WaitForSeconds(SpawnDuration);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = true;
        }
        Debug.Log("Finish spawn on " + go.name);
        OnSpawnFinished?.Invoke(go);
    }
}
