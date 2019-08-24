using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner spriteDirectioner;
    [SerializeField]
    private PlayerData _playerData;

    private Vector2 _moveDirection;


    private void Awake()
    {
        _walkManager.Init(_playerData.WalkingAnimation);
        spriteDirectioner.Init(_playerData.CardinalSprite);
    }

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
