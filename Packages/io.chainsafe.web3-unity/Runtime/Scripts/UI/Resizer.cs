using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    /// <summary>
    /// Resizes a rect Transform responsively/based on percentage.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class Resizer : MonoBehaviour
    {
        [SerializeField] private bool resizeWidth;
        [SerializeField] private bool resizeHeight;

        [Space]

        [SerializeField] private float widthThreshold;
        [SerializeField] private float heightThreshold;

        [Space]

        [SerializeField] private bool resizeRelativeToParent;

        [Space]

        [Range(0f, 1f)]
        [SerializeField] private float normalizedWidth;
        [Range(0f, 1f)]
        [SerializeField] private float normalizedHeight;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            Resize();
        }

        private void Resize()
        {
            Vector2 parentSize = resizeRelativeToParent ? ((RectTransform)_rectTransform.parent).rect.size : new Vector2(Screen.width, Screen.height);

            if (resizeWidth)
            {
                ResizeAxis(RectTransform.Axis.Horizontal, parentSize.x, normalizedWidth, widthThreshold);
            }

            if (resizeHeight)
            {
                ResizeAxis(RectTransform.Axis.Vertical, parentSize.y, normalizedHeight, heightThreshold);
            }

            void ResizeAxis(RectTransform.Axis axis, float parent, float normalized, float threshold)
            {
                float target = parent * normalized;

                if (threshold > 0)
                {
                    target = Mathf.Clamp(target, 0f, threshold);
                }

                _rectTransform.SetSizeWithCurrentAnchors(axis, target);
            }
        }
    }
}
