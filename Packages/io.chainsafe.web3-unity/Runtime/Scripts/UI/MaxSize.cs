using System;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(LayoutGroup))]
    public class MaxSize : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float normalizedMaxWidth;
        
        [SerializeField, Range(0f, 1f)] private float normalizedMaxHeight;

        private LayoutGroup _layout;
        
        private RectTransform _rectTransform;
        
        private RectTransform _parent;
        
        private void OnEnable()
        {
            _layout = GetComponent<LayoutGroup>();
            
            _rectTransform = GetComponent<RectTransform>();

            _parent = (RectTransform) transform.parent;
        }

        private void Update()
        {
            if (normalizedMaxWidth > 0)
            {
                float preferredWidth = _layout.preferredWidth;

                float maxWidth = _parent.rect.width * normalizedMaxWidth;
            
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Min(preferredWidth, maxWidth));
            }
            
            if (normalizedMaxHeight > 0)
            {
                float preferredHeight = _layout.preferredHeight;

                float maxHeight = _parent.rect.height * normalizedMaxHeight;
            
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Min(preferredHeight, maxHeight));
            }
        }
    }
}
