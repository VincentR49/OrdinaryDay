using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
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


    public void Spawn(GameObject go, SpriteDirectioner spriteDirectioner, List<MonoBehaviour> behaviourToDisable = null)
    {
        StartCoroutine(SpawnCoroutine(go, spriteDirectioner, behaviourToDisable));
    }


    private IEnumerator SpawnCoroutine(GameObject go, SpriteDirectioner spriteDirectioner, List<MonoBehaviour> disableDuringSpawn = null)
    {
        Debug.Log("Start spawn coroutine");
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = false;
        }
        go.transform.position = gameObject.transform.position;
        spriteDirectioner.SetSprite(_spawnDirection);
        yield return new WaitForSeconds(SpawnDuration);
        if (disableDuringSpawn != null)
        {
            foreach (var behaviour in disableDuringSpawn)
                behaviour.enabled = true;
        }
        Debug.Log("Finish spawn !");
    }
}
