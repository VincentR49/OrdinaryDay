using UnityEngine;

public class CameraPlayerFollower : MonoBehaviour
{
    private const string PlayerTag = "Player";

    private GameObject _target;
    private Camera _camera;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _target = LookForTarget();
        FollowTarget();
    }


    private GameObject LookForTarget()
    {
        return GameObject.FindWithTag(PlayerTag);
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = LookForTarget();
        }
        FollowTarget();
    }



    private void FollowTarget()
    {
        if (_target == null)
        {
            // do something
        }
        else
        {
            var x = _target.transform.position.x;
            var y = _target.transform.position.y;
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
