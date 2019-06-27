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

    public bool IsFadingIn { private set; get; }
    public bool IsFadingOut { private set; get; }
    public float FadeDuration => FadeOriginalDuration / _animator.speed;

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void FadeIn() => FadeIn(_defaultFadeDuration);
    public void FadeOut() => FadeOut(_defaultFadeDuration);

    public void FadeIn(float duration)
    {
        SetFadeDuration(duration);
        Debug.Log("Start fadeIn: duration " + FadeDuration);
        _animator.SetTrigger("FadeIn");
        IsFadingIn = true;
    }

    public void FadeOut(float duration)
    {
        SetFadeDuration(duration);
        Debug.Log("Start fadeOut: duration " + FadeDuration);
        _animator.SetTrigger("FadeOut");
        IsFadingOut = true;
    }


    private void OnFadeInFinished()
    {
        Debug.Log("OnFadeInFinished");
        FadeInFinished.Invoke();
        IsFadingIn = false;
    }


    private void OnFadeOutFinished()
    {
        Debug.Log("OnFadeOutFinished");
        FadeOutFinished.Invoke();
        IsFadingOut = false;
    }


    private void SetFadeDuration(float duration)
    {
        _animator.speed = FadeOriginalDuration / duration;
    }
}
