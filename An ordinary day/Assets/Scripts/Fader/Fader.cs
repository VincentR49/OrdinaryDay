using UnityEngine;
using UnityEngine.Events;

public class Fader : MonoBehaviour
{
    private const float FadeOriginalDuration = 1f; // duration in animator

    [SerializeField]
    private float _defaultFadeDuration = FadeOriginalDuration;

    protected Animator _animator;
    public UnityEvent FadeInFinished;
    public UnityEvent FadeOutFinished;

    private bool _isFadingIn;
    private bool _isFadingOut;
    private float _fadeDuration => FadeOriginalDuration / _animator.speed;


    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void FadeIn() => FadeIn(_defaultFadeDuration);
    public void FadeOut() => FadeOut(_defaultFadeDuration);

    public void FadeIn(float duration)
    {
        SetFadeDuration(duration);
        Debug.Log("Start fadeIn: duration " + _fadeDuration);
        _animator.SetTrigger("FadeIn");
        _isFadingIn = true;
    }

    public void FadeOut(float duration)
    {
        SetFadeDuration(duration);
        Debug.Log("Start fadeOut: duration " + _fadeDuration);
        _animator.SetTrigger("FadeOut");
        _isFadingOut = true;
    }


    private void OnFadeInFinished()
    {
        Debug.Log("OnFadeInFinished");
        FadeInFinished.Invoke();
        _isFadingIn = false;
    }


    private void OnFadeOutFinished()
    {
        Debug.Log("OnFadeOutFinished");
        FadeOutFinished.Invoke();
        _isFadingOut = false;
    }


    private void SetFadeDuration(float duration)
    {
        _animator.speed = FadeOriginalDuration / duration;
    }
}
