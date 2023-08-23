using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SampleFeedback : MonoBehaviour
{
    public Animator animator;
    public Animation LoadingAnimation;
    public EventSystem eventSystem;

    public static SampleFeedback Instance { get; private set; }

    private void OnEnable()
    {
        if (Instance)
        {
            throw new Exception($"Tried activating second instance of {nameof(SampleFeedback)}");
        }

        Instance = this;
    }

    private void OnDisable()
    {
        if (Instance != this)
            return;

        Instance = null;
    }

    public void Activate()
    {
        eventSystem.enabled = false;
        animator.SetBool("On", true);
        LoadingAnimation.Play();
    }

    public void Deactivate()
    {
        eventSystem.enabled = true;
        animator.SetBool("On", false);
        LoadingAnimation.Stop();
        LoadingAnimation.Rewind();
    }
}
