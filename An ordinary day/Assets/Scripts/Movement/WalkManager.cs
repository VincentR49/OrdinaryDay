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
    private GroundMultiplier[] _groundMultipliers;

    [Serializable]
    private class GroundMultiplier
    {
        public string GroundTag;
        public float Multiplier;
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
    private bool HasGroundMultiplier => _groundMultipliers != null && _groundMultipliers.Length > 0;
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
        if (!HasGroundMultiplier)
            return;
        //Debug.Log("On trigger enter: " + collision.tag);
        _currentTriggers.Add(collision);
        UpdateCurrentGroundMultiplier();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!HasGroundMultiplier)
            return;
        //Debug.Log("On trigger exit: " + collision.tag);
        ResetSpeedMultiplier();
        _currentTriggers.Remove(collision);
        UpdateCurrentGroundMultiplier();
    }


    private void UpdateCurrentGroundMultiplier()
    {
        var nTriggers = _currentTriggers.Count;
        if (nTriggers == 0) // in contact with no trigger
        {
            ResetSpeedMultiplier();
            return;
        }
        // We take into account the last trigger collided as reference for the speed multiplier
        var lastTriggerHit = _currentTriggers[nTriggers - 1];
        var groundMultiplier = _groundMultipliers.FirstOrDefault((x) => x.GroundTag.Equals(lastTriggerHit.tag));
        if (groundMultiplier != null)
            _speedMultiplier = groundMultiplier.Multiplier;
        else
            ResetSpeedMultiplier();
    }

    private void ResetSpeedMultiplier()
    {
        _speedMultiplier = 1f;
    }

    #endregion
}
