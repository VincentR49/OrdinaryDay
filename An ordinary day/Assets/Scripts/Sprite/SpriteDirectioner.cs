using System.Collections;
using System.Collections.Generic;
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

    public void SetDirection(Direction direction)
    {
        var sprite = _cardinalSprite.GetSprite(direction);
        _spriteRenderer.flipX = false;
        if (sprite == null)
        {
            if ((direction == Direction.Right || direction == Direction.Left)
                        && _cardinalSprite.FlipXForRightOrLeft)
            {
                sprite = _cardinalSprite.GetSprite(direction == Direction.Right ? Direction.Left : Direction.Right);
                _spriteRenderer.flipX = true;
            }
            else
            {
                Debug.LogError("No sprite found.");
                return;
            }
        }
        _spriteRenderer.sprite = sprite;
    }
}
