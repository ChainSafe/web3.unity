using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    /// <summary>
    /// SafeArea Container for UI Toolkit.
    /// </summary>
    public class SafeArea : VisualElement
    {
        private struct Offset
        {
            public float Left, Right, Top, Bottom;

            public override string ToString()
            {
                return $"l: {Left}, r: {Right}, t:{Top}, b: {Bottom}";
            }
        }

        public new class UxmlFactory : UxmlFactory<SafeArea, UxmlTraits>
        {
        }

        public bool CollapseMargins { get; set; }
        public bool ExcludeLeft { get; set; }
        public bool ExcludeRight { get; set; }
        public bool ExcludeTop { get; set; }
        public bool ExcludeBottom { get; set; }
        public bool ExcludeTvos { get; set; }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlBoolAttributeDescription _collapseMarginsAttr = new() { name = "collapse-margins", defaultValue = true };
            private UxmlBoolAttributeDescription _excludeLeftAttr = new() { name = "exclude-left", defaultValue = false };
            private UxmlBoolAttributeDescription _excludeRightAttr = new() { name = "exclude-right", defaultValue = false };
            private UxmlBoolAttributeDescription _excludeTopAttr = new() { name = "exclude-top", defaultValue = false };
            private UxmlBoolAttributeDescription _excludeBottomAttr = new() { name = "exclude-bottom", defaultValue = false };
            private UxmlBoolAttributeDescription _excludeTvosAttr = new() { name = "exclude-tvos", defaultValue = false };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as SafeArea;

                ate.CollapseMargins = _collapseMarginsAttr.GetValueFromBag(bag, cc);
                ate.ExcludeLeft = _excludeLeftAttr.GetValueFromBag(bag, cc);
                ate.ExcludeRight = _excludeRightAttr.GetValueFromBag(bag, cc);
                ate.ExcludeTop = _excludeTopAttr.GetValueFromBag(bag, cc);
                ate.ExcludeBottom = _excludeBottomAttr.GetValueFromBag(bag, cc);
                ate.ExcludeTvos = _excludeTvosAttr.GetValueFromBag(bag, cc);
            }
        }

        private VisualElement _contentContainer;
        public override VisualElement contentContainer => _contentContainer;

        public SafeArea()
        {
            // By using absolute position instead of flex to fill the full screen, SafeArea containers can be stacked.
            style.position = Position.Absolute;
            style.top = 0;
            style.bottom = 0;
            style.left = 0;
            style.right = 0;
            pickingMode = PickingMode.Ignore;

            _contentContainer = new VisualElement();
            _contentContainer.name = "safe-area-content-container";
            _contentContainer.pickingMode = PickingMode.Ignore;
            _contentContainer.style.flexGrow = 1;
            _contentContainer.style.flexShrink = 0;
            hierarchy.Add(_contentContainer);

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            // As RuntimePanelUtils is not available in UIBuilder,
            // the handling is wrapped in a try/catch to avoid InvalidCastExceptions when working in UIBuilder.
            try
            {
                var safeArea = GetSafeAreaOffset();
                var margin = GetMarginOffset();

                if (CollapseMargins)
                {
                    _contentContainer.style.marginLeft = Mathf.Max(margin.Left, safeArea.Left) - margin.Left;
                    _contentContainer.style.marginRight = Mathf.Max(margin.Right, safeArea.Right) - margin.Right;
                    _contentContainer.style.marginTop = Mathf.Max(margin.Top, safeArea.Top) - margin.Top;
                    _contentContainer.style.marginBottom = Mathf.Max(margin.Bottom, safeArea.Bottom) - margin.Bottom;
                }
                else
                {
                    _contentContainer.style.marginLeft = safeArea.Left;
                    _contentContainer.style.marginRight = safeArea.Right;
                    _contentContainer.style.marginTop = safeArea.Top;
                    _contentContainer.style.marginBottom = safeArea.Bottom;
                }
            }
            catch (System.InvalidCastException)
            {
            }
        }

        // Convert screen safe area to panel space and get the offset values from the panel edges.
        private Offset GetSafeAreaOffset()
        {
            var safeArea = Screen.safeArea;
            var leftTop = RuntimePanelUtils.ScreenToPanel(panel, new Vector2(safeArea.xMin, Screen.height - safeArea.yMax));
            var rightBottom = RuntimePanelUtils.ScreenToPanel(panel, new Vector2(Screen.width - safeArea.xMax, safeArea.yMin));

#if UNITY_TVOS
            if (ExcludeTvos)
                return new Offset { Left = 0, Right = 0, Top = 0, Bottom = 0 };
#endif

            // If the user has flagged an edge as excluded, set that edge to 0.
            return new Offset
            {
                Left = ExcludeLeft ? 0 : leftTop.x,
                Right = ExcludeRight ? 0 : rightBottom.x,
                Top = ExcludeTop ? 0 : leftTop.y,
                Bottom = ExcludeBottom ? 0 : rightBottom.y
            };
        }

        // Get the resolved margins from the inline style.
        private Offset GetMarginOffset()
        {
            return new Offset()
            {
                Left = resolvedStyle.marginLeft,
                Right = resolvedStyle.marginRight,
                Top = resolvedStyle.marginTop,
                Bottom = resolvedStyle.marginBottom
            };
        }
    }
}