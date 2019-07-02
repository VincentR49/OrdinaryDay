using UnityEngine;
using System.Collections;

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


    public void Spawn(GameObject go, SpriteDirectioner spriteDirectioner)
    {
        StartCoroutine(SpawnCoroutine(go, spriteDirectioner));
    }


    private IEnumerator SpawnCoroutine(GameObject go, SpriteDirectioner spriteDirectioner)
    {
        Debug.Log("Start spawn coroutine");
        go.SetActive(false);
        yield return new WaitForSeconds(SpawnDuration);
        go.transform.position = gameObject.transform.position;
        spriteDirectioner.SetDirection(_spawnDirection);
        go.SetActive(true);
        Debug.Log("Finish spawn !");
    }
}
