using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class Modal : VisualElement
    {
        public const string Name = "modal";
        public static readonly string NameBody = $"{Name}__body";

        public static readonly string ClassNameTypeDesktop = $"{Name}--type-desktop";
        public static readonly string ClassNameTypeMobile = $"{Name}--type-mobile";
        public static readonly string ClassNameLandscape = $"{Name}--landscape";

        public readonly ModalHeader header;
        public readonly VisualElement body;

        public new class UxmlFactory : UxmlFactory<Modal>
        {
        }

        public Modal()
        {
            var asset = Resources.Load<VisualTreeAsset>("Reown/AppKit/Components/Modal/Modal");
            asset.CloneTree(this);

            name = Name;

#if UNITY_IOS || UNITY_ANDROID
            AddToClassList(ClassNameTypeMobile);
#else
            AddToClassList(ClassNameTypeDesktop);
#endif

            header = this.Q<ModalHeader>();
            body = this.Q<VisualElement>(NameBody);

            // Scale the modal to fit the content
            body?.RegisterCallback<GeometryChangedEvent, Modal>(
                (evt, modal) =>
                {
                    var newHeight = Mathf.CeilToInt(evt.newRect.height + header.resolvedStyle.height
#if UNITY_ANDROID || UNITY_IOS
                        // Bottom safe area
                        + RuntimePanelUtils.ScreenToPanel(panel,
                            new Vector2(Screen.width - Screen.safeArea.xMax, Screen.safeArea.yMin)
                        ).y
#endif
                    );

                    if (modal.style.height == newHeight)
                        return;

                    // Prevent bouncing
                    if (Mathf.Abs(newHeight - Mathf.RoundToInt(modal.style.height.value.value)) < 2)
                        return;

                    modal.style.height = newHeight;
                }, this
            );

#if UNITY_IOS || UNITY_ANDROID
            // Landscape layout
            ConfigureLandscape(Screen.orientation);
            Utils.OrientationTracker.OrientationChanged += (_, orientation) => ConfigureLandscape(orientation);
#endif
        }

#if UNITY_IOS || UNITY_ANDROID
        private void ConfigureLandscape(ScreenOrientation orientation)
        {
            if (orientation is ScreenOrientation.LandscapeLeft or ScreenOrientation.LandscapeRight)
            {
                AddToClassList(ClassNameLandscape);
            }
            else
            {
                if (ClassListContains(ClassNameLandscape))
                    RemoveFromClassList(ClassNameLandscape);
            }
        }
#endif
    }
}