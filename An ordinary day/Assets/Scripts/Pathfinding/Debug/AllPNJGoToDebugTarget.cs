using UnityEngine;
using System.Collections;

public class AllPNJGoToDebugTarget : MonoBehaviour
{
    private TargetReacher[] _pnjTargetReacher;
    [SerializeField]
    private Transform _target;
    public bool OnStart = false;

    private void Start()
    {
        _pnjTargetReacher = FindObjectsOfType<TargetReacher>();
        if (OnStart)
            StartCoroutine(GoToTargetRoutine());
    }


    public void GoToTarget()
    {
        foreach (var pnjController in _pnjTargetReacher)
            pnjController.GoToTarget(_target.position);
    }


    private IEnumerator GoToTargetRoutine()
    {
        yield return new WaitForSeconds(1);
        GoToTarget();
    }
}
