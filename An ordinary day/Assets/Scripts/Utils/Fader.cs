using UnityEngine;
using UnityEngine.Events;

public class Fader : MonoBehaviour
{
    private Animator _animator;
    public UnityEvent FadeInFinished;
    public UnityEvent FadeOutFinished;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void FadeIn()
    {
        _animator.SetTrigger("FadeIn");
    }


    public void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }


    private void OnFadeInFinished()
    {
        Debug.Log("OnFadeInFinished");
        FadeInFinished.Invoke();
    }


    private void OnFadeOutFinished()
    {
        Debug.Log("OnFadeOutFinished");
        FadeOutFinished.Invoke();
    }
}
