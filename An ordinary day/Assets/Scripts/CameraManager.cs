using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private bool _followTarget;


    private void Update()
    {
        if (_followTarget)
            FollowTarget();
    }


    private void FollowTarget()
    {
        var x = _target.transform.position.x;
        var y = _target.transform.position.y;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
