using UnityEngine;

// Set the correct sprite according to the direction of the object
public class SpriteDirectioner : MonoBehaviour
{
    private const Direction InitDirection = Direction.South;
    private CardinalSpriteData _cardinalSprite;
    private SpriteRenderer _spriteRenderer;
    private Direction _direction;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Direction direction)
    {
        if (_cardinalSprite == null)
        {
            Debug.LogError("Cardinal sprite not set.");
            return;
        }
        _direction = direction;
        var sprite = _cardinalSprite.Get(direction);
        _spriteRenderer.flipX = sprite.FlipX;
        _spriteRenderer.sprite = sprite.Sprite;
    }

    public void Init(CardinalSpriteData sprite)
    {
        _cardinalSprite = sprite;
        SetDirection(InitDirection);
    }
}
