using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SampleFeedback : MonoBehaviour
{
    public Animator animator;
    public Animation LoadingAnimation;
    public EventSystem eventSystem;

    [SerializeField] private TMP_Text _messageLabel;

    public static SampleFeedback Instance { get; private set; }

    private Coroutine _showMessageCoroutine;

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

        ClearMessage();
    }

    public void Deactivate()
    {
        eventSystem.enabled = true;
        animator.SetBool("On", false);
        LoadingAnimation.Stop();
        LoadingAnimation.Rewind();
    }

    public void ShowMessage(string message, Color color = default, float timeout = 4f)
    {
        if (color == default)
            color = Color.white;

        //stop any previous messages being displayed
        ClearMessage();

        _showMessageCoroutine = StartCoroutine(DisplayMessage(message, color, timeout));
    }

    private IEnumerator DisplayMessage(string message, Color color, float timeout)
    {
        _messageLabel.text = message;

        _messageLabel.color = color;

        yield return new WaitForSeconds(timeout);

        _messageLabel.text = string.Empty;
    }

    private void ClearMessage()
    {
        if (_showMessageCoroutine != null)
        {
            StopCoroutine(_showMessageCoroutine);

            _showMessageCoroutine = null;
        }

        _messageLabel.text = string.Empty;
    }
}
