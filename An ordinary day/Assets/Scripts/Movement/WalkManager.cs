using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Deal with the change of position of an object and displayed the correct animation according to its movement.
/// Can change the speed in function of where the object walk.
/// </summary>
public class WalkManager : MonoBehaviour
{
    private const float Epsilon = 0.001f;

    [Header("Speed")]
    [SerializeField]
    private float _defaultSpeed = 3f;
    [SerializeField]
    private Ground[] _grounds;

    [Serializable]
    private class Ground
    {
        public string GroundTag;
        public float Multiplier = 1f;
    }

    [Header("Display")]
    [SerializeField]
    private SpriteAnimator _spriteAnimator = default;
    [SerializeField]
    private CardinalAnimationData _walkAnimation;

    private Rigidbody2D _rb;

    // speed management data
    private float Speed => _defaultSpeed * _speedMultiplier;
    private float _speedMultiplier = 1f;
    private bool HasGroundsDefined => _grounds != null && _grounds.Length > 0;
    private List<Collider2D> _currentTriggers = new List<Collider2D>();

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
        _rb.MovePosition(_rb.position + direction * Speed * Time.deltaTime);
    }

    #region State management
    public void Stop()
    {
        ChangeState(State.Stop);
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
    #endregion


    #region Animation
    private void Animate(State state)
    {
        switch (state)
        {
            case State.Top:
                _spriteAnimator.StartAnimation(_walkAnimation.Get(Direction.Top));
                break;
            case State.Down:
                _spriteAnimator.StartAnimation(_walkAnimation.Get(Direction.Bottom));
                break;
            case State.Left:
                _spriteAnimator.StartAnimation(_walkAnimation.Get(Direction.Left));
                break;
            case State.Right:
                _spriteAnimator.StartAnimation(_walkAnimation.Get(Direction.Right));
                break;
            case State.Stop:
            default:
                _spriteAnimator.StopCurrentAnimation();
                break;
        }
    }


    public void SetWalkAnimation(CardinalAnimationData walkAnimation)
    {
        _walkAnimation = walkAnimation;
    }
    #endregion


    #region Speed

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!HasGroundsDefined)
            return;
        //Debug.Log("On trigger enter: " + collision.tag);
        _currentTriggers.Add(collision);
        UpdateCurrentGroundMultiplier();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!HasGroundsDefined)
            return;
        //Debug.Log("On trigger exit: " + collision.tag);
        _currentTriggers.Remove(collision);
        UpdateCurrentGroundMultiplier();
    }


    private void UpdateCurrentGroundMultiplier()
    {
        // We take the most profitable ground currently in contact with the player
        var specialGroundFound = false;
        foreach (var trigger in _currentTriggers)
        {
            var ground = _grounds.FirstOrDefault((x) => x.GroundTag.Equals(trigger.tag));
            if (ground != null)
            {
                specialGroundFound = true;
                _speedMultiplier = Math.Max(_speedMultiplier, ground.Multiplier);
            }
        }
        if (!specialGroundFound)
            ResetSpeedMultiplier();
    }

    private void ResetSpeedMultiplier()
    {
        //Debug.Log("ResetSpeedMultiplier");
        _speedMultiplier = 1f;
    }

    #endregion
}
