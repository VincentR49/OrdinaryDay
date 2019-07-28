using UnityEngine;

public class PNJInstancier : MonoBehaviour
{
    [SerializeField]
    private PNJController _pnjPrefab;
    [SerializeField]
    private PNJData _pnjData;
    [SerializeField]
    private SpawnPoint spawnPoint;


    private void Start()
    {
        var pnj = Instantiate(_pnjPrefab);
        pnj.Init(_pnjData);
        spawnPoint.Spawn(pnj.gameObject);
    }
}
