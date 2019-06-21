﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    private float _timeBetweenFrames = 0.1f;

    private bool IsAnimationRunning => _currentAnimation != null;

    private Coroutine _currentAnimationRoutine;
    private AnimationData _currentAnimation;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void StartAnimation(AnimationData animation)
    {
        StopCurrentAnimation();
        if (animation == null)
        {
            Debug.LogError("Cannot run null animation");
        }
        else
        {
            _currentAnimationRoutine = StartCoroutine(AnimationCoroutine(animation));
        }
    }


    public void StopCurrentAnimation()
    {
        if (IsAnimationRunning)
        {
            if (_currentAnimation.ReturnToFirstSpriteWhenFinished)
                _spriteRenderer.sprite = _currentAnimation.Sprites[0];
            _currentAnimation = null;
            StopCoroutine(_currentAnimationRoutine);
            Debug.Log("StopCurrentAnimation");
        }
    }


    private IEnumerator AnimationCoroutine(AnimationData animationData)
    {
        _currentAnimation = animationData;
        var nFrames = animationData.Sprites.Length;
        var frame = 0;
        _spriteRenderer.flipX = animationData.FlipX;
        while (true)
        {
            _spriteRenderer.sprite = animationData.Sprites[frame];
            frame++;
            if (frame == nFrames)
            {
                if (animationData.Loop)
                    frame = 0;
                else
                    StopCurrentAnimation();
            }
            yield return new WaitForSeconds(_timeBetweenFrames);
        }
    }
}
