using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPNJGoToDebugTarget : MonoBehaviour
{
    private PNJController[] _pnjControllers;

    private void Start()
    {
        _pnjControllers = FindObjectsOfType<PNJController>();
    }


    public void GoToTarget()
    {
        foreach (var pnjController in _pnjControllers)
            pnjController.GoToDebugTarget();
    }
}
