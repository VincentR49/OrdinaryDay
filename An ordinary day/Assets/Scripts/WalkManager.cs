using UnityEngine;

public class WalkManager : MonoBehaviour
{
    private const float Epsilon = 0.001f;
    [SerializeField]
    private float _speed = default;
    [SerializeField]
    private SpriteAnimator _spriteAnimator = default;

    [Header("Walk Animations")]
    [SerializeField]
    private AnimationData _leftAnimation = default;
    [SerializeField]
    private AnimationData _rightAnimation = default;
    [SerializeField]
    private AnimationData _topAnimation = default;
    [SerializeField]
    private AnimationData _downAnimation = default;

    private Rigidbody2D _rb;

    private enum State
    {
        Stop,
        Left,
        Right,
        Top,
        Down
    }

    private State _state = State.Stop;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        direction.Normalize();
        //Debug.Log(direction);
        var state = GetState(direction);
        if (state != _state)
        {
            ChangeState(state);
        }
        _rb.MovePosition(_rb.position + direction * _speed * Time.deltaTime);
    }


    private State GetState(Vector2 direction)
    {
        var absX = Mathf.Abs(direction.x);
        var absY = Mathf.Abs(direction.y);
        if (absX < Epsilon && absY < Epsilon)
            return State.Stop;
        else if (absX >= absY) 
            return direction.x > 0 ? State.Right : State.Left;
        else 
            return direction.y > 0 ? State.Top : State.Down;

    }


    private void ChangeState(State state)
    {
        _state = state;
        //Debug.Log("Change State: " + state);
        Animate(state);
    }


    private void Animate(State state)
    {
        switch (state)
        {
            case State.Top:
                _spriteAnimator.StartAnimation(_topAnimation);
                break;
            case State.Down:
                _spriteAnimator.StartAnimation(_downAnimation);
                break;
            case State.Left:
                _spriteAnimator.StartAnimation(_leftAnimation);
                break;
            case State.Right:
                _spriteAnimator.StartAnimation(_rightAnimation);
                break;
            case State.Stop:
            default:
                _spriteAnimator.StopCurrentAnimation();
                break;
        }
    }
}
