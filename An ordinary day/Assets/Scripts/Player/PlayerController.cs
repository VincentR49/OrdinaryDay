using UnityEngine;


/// <summary>
/// Manage the player controls (key input)
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private WalkManager _walkManager;
    [SerializeField]
    private SpriteDirectioner _spriteDirectioner;
    [SerializeField]
    private PlayerData _playerData;
    [SerializeField]
    private SpeakToNPCStarter _speakToNPCStarter;

    private Vector2 _moveDirection;


    private void Awake()
    {
        _walkManager.Init(_playerData.WalkingAnimation);
        _spriteDirectioner.Init(_playerData.CardinalSprite);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GamePauser.IsPaused)
        {
            return;
        } 
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space)) // Interaction Key
        {
            // Check for Npc to speak to
            var couldSpeakToNpc = _speakToNPCStarter.StartDialogueIfPossible();
            if (couldSpeakToNpc)
            {
                _moveDirection = Vector2.zero;
                return;
            }
            // Interact with objects
        }
    }


    private void LateUpdate()
    {
        //Debug.Log("Move direction: " + _moveDirection);
        _walkManager.Move(_moveDirection);
    }
}
