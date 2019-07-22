using UnityEngine;

// Set the correct sprite according to the direction of the object
public class SpriteDirectioner : MonoBehaviour
{
    [SerializeField]
    private CardinalSpriteData _cardinalSprite = default;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Direction direction)
    {
        var sprite = _cardinalSprite.Get(direction);
        _spriteRenderer.flipX = sprite.FlipX;
        _spriteRenderer.sprite = sprite.Sprite;
    }

    public void SetCardinalSprite(CardinalSpriteData sprite)
    {
        _cardinalSprite = sprite;
    }
}
