using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFade : MonoBehaviour
{
    public delegate void OnFadeComplete();

    public List<OnFadeComplete> OnFadeCompleteCallbacks { get; set; }

    public float secondsToFade = 0.5f;

    private Animator _animator;

    private void Awake()
    {
        OnFadeCompleteCallbacks = new List<OnFadeComplete>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void FadeToBlack()
    {
        if (_animator)
        {
            _animator.speed = 1.0f / secondsToFade;
            _animator.SetTrigger("FadeToBlack");
            StartCoroutine(HandleCallbacks(secondsToFade));
        }
    }

    public void FadeFromBlack()
    {
        if (_animator)
        {
            _animator.speed = 1.0f / secondsToFade;
            _animator.SetTrigger("FadeFromBlack");
            StartCoroutine(HandleCallbacks(secondsToFade));
        }
    }

    private IEnumerator HandleCallbacks(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        foreach (var callback in OnFadeCompleteCallbacks)
            callback();
    }
}
