using UnityEngine;

/// <summary>
/// Player teleport behaviour but with restricting orientation to make it work.
/// The player has to face the teleport in the given direction in order to activate it
/// </summary>
public class DirectionalPlayerTeleporter : PlayerTeleporter
{
    [SerializeField]
    private Direction _startTeleportDirection;


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // do nothing
        // override onTriggerENter of base class
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isTeleporting || !IsPlayer(collision))
            return;
        var v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Utils.GetDirection(v) == _startTeleportDirection)
        {
            _isTeleporting = true;
            TeleportPlayer(collision.gameObject);
        }
    }
}
