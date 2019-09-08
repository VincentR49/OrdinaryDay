using System.Collections;
using UnityEngine;

public class InventoryDebugScript : MonoBehaviour
{
    [SerializeField]
    private RuntimeInventory _playerInventory;
    [SerializeField]
    private GameItemData[] _testObjects;

    [SerializeField]
    private bool _onStart;


    private void Start()
    {
        if (_onStart)
            StartCoroutine(TestRoutine());
    }


    private IEnumerator TestRoutine()
    {
        yield return new WaitForSeconds(2);
        yield return AddObjectRoutine();
        yield return new WaitForSeconds(2);
        yield return RemoveObjectRoutine();
        yield return new WaitForSeconds(2);
        yield return AddObjectRoutine();
    }



    private IEnumerator AddObjectRoutine()
    {
        Debug.Log("AddObjectRoutine");
        foreach (var item in _testObjects)
        {
            _playerInventory.AddItem(item);
            yield return new WaitForSeconds(1);
        }
    }


    private IEnumerator RemoveObjectRoutine()
    {
        Debug.Log("RemoveObjectRoutine");
        foreach (var item in _testObjects)
        {
            _playerInventory.RemoveItem(item);
            yield return new WaitForSeconds(1);
        }
    }
}
