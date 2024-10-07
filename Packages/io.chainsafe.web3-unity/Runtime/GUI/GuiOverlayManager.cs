using System;
using UnityEngine;

namespace ChainSafe.Gaming.GUI
{
    public class GuiOverlayManager : MonoBehaviour
    {
        public GuiScreenFactory ScreenFactory;
        private Action onClose;

        private GuiInfoOverlay Overlay => ScreenFactory.GetSingle<GuiInfoOverlay>(); // todo use pool

        public void Show(GuiOverlayType type, string message, bool deactivateOnClick, Action onClose = null)
        {
            this.onClose = onClose;
            Overlay.Initialize(type, message, deactivateOnClick);
            Overlay.gameObject.SetActive(true);
        }

        public void Hide()
        {
            Overlay.gameObject.SetActive(false);
            
            if (onClose != null)
            {
                onClose.Invoke();
                onClose = null;
            }
        }
    }

    public enum GuiOverlayType
    {
        Error,
        Loading
    }
}