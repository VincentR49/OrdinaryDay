using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private WalkManager _walkManager = default;

    private Vector2 _moveDirection;

    // Update is called once per frame
    private void Update()
    {
        _moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        _walkManager.Move(_moveDirection);
    }
}
