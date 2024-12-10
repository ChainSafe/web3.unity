using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Utils
{
    public class LoadingAnimator : MonoBehaviour
    {
        [SerializeField] private Color _colorA;
        [SerializeField] private Color _colorB;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private AnimationCurve _lerpCurve;

        private readonly HashSet<VisualElement> _subscribedVisualElements = new();

        private bool _isAnimating;
        private bool _isPaused;

        public static LoadingAnimator Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Subscribe<T>(T element) where T : class
        {
            switch (element)
            {
                case VisualElement visualElement:
                    _subscribedVisualElements.Add(visualElement);
                    break;
            }

            if (!_isAnimating && !_isPaused)
                StartAnimation();
        }

        public void Unsubscribe<T>(T element) where T : class
        {
            switch (element)
            {
                case VisualElement visualElement:
                    _subscribedVisualElements.Remove(visualElement);
                    break;
            }

            if (_subscribedVisualElements.Count == 0)
                StopAnimation();
        }

        private IEnumerator AnimateColorRoutine()
        {
            var t = 0f;
            _isAnimating = true;

            while (_isAnimating)
            {
                if (_isPaused)
                    yield return new WaitUntil(() => !_isPaused);

                var currentColor = Color.Lerp(_colorA, _colorB, _lerpCurve.Evaluate(t));
                t += Time.deltaTime * _speed;
                if (t > 1f)
                {
                    t = 0f;
                    (_colorA, _colorB) = (_colorB, _colorA);
                }

                foreach (var visualElement in _subscribedVisualElements)
                    visualElement.style.backgroundColor = currentColor;

                yield return null;
            }
        }

        public void PauseAnimation()
        {
            _isPaused = true;
        }

        public void ResumeAnimation()
        {
            if (!_isPaused)
                return;

            _isPaused = false;

            if (!_isAnimating && _subscribedVisualElements.Count > 0)
                StartAnimation();
        }

        private void StartAnimation()
        {
            StartCoroutine(AnimateColorRoutine());
        }

        private void StopAnimation()
        {
            _isAnimating = false;
            StopAllCoroutines();
        }
    }
}