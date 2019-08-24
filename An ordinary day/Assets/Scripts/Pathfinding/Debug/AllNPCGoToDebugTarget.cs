using UnityEngine;
using System.Collections;

public class AllNPCGoToDebugTarget : MonoBehaviour
{
    private TargetReacher[] _targetReachers;
    [SerializeField]
    private Transform _target;
    public bool OnStart = false;

    private void Start()
    {
        _targetReachers = FindObjectsOfType<TargetReacher>();
        if (OnStart)
            StartCoroutine(GoToTargetRoutine());
    }


    public void GoToTarget()
    {
        foreach (var targetReacher in _targetReachers)
            targetReacher.GoToTarget(_target.position);
    }


    private IEnumerator GoToTargetRoutine()
    {
        yield return new WaitForSeconds(1);
        GoToTarget();
    }
}
