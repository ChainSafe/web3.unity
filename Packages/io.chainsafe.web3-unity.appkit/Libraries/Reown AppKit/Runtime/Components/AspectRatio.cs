using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class AspectRatio : VisualElement
    {
        private float _lastParentWidth;

        public new class UxmlFactory : UxmlFactory<AspectRatio>
        {
        }

        public AspectRatio()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanelEvent);
        }

        public AspectRatio(VisualElement child) : this()
        {
            Add(child);
        }

        private void OnAttachToPanelEvent(AttachToPanelEvent e)
        {
            parent?.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
            FitToParent();
        }

        private void OnGeometryChangedEvent(GeometryChangedEvent e)
        {
            if (e.newRect.height == 0) return;
            FitToParent();
        }

        private void FitToParent()
        {
            if (parent == null)
                return;

            var parentWidth = parent.resolvedStyle.width;

            if (Mathf.Approximately(parentWidth, _lastParentWidth))
                return;

            var parentPaddingLeft = parent.resolvedStyle.paddingLeft;
            var parentPaddingRight = parent.resolvedStyle.paddingRight;

            parentWidth -= parentPaddingLeft + parentPaddingRight;

            style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            style.height = parentWidth;
            style.flexGrow = 0;

            _lastParentWidth = parentWidth;
        }
    }
}