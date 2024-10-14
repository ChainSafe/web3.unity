using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.GUI
{
    public class GuiInfoOverlay : MonoBehaviour
    {
        public TMP_Text Message;
        public GameObject ErrorIcon;
        public GameObject LoadingIcon;
        public Button CloseButton;
        
        private bool closeOnClick;
        private Action onClose;
        private Action<GuiInfoOverlay> onRelease;

        public int Id { get; private set; }

        private void Awake()
        {
            CloseButton.onClick.AddListener(OnScreenClick);
        }

        public void Initialize(int id, GuiOverlayType type, string message, bool closeOnClick, Action onClose, Action<GuiInfoOverlay> release)
        {
            Id = id;
            this.onClose = onClose;
            this.closeOnClick = closeOnClick;
            onRelease = release;
            
            ErrorIcon.SetActive(type == GuiOverlayType.Error);
            LoadingIcon.SetActive(type == GuiOverlayType.Loading);
            Message.text = message;
        }

        private void OnScreenClick()
        {
            if (!closeOnClick)
            {
                return;
            }
            
            Hide();
        }

        internal void Hide()
        {
            gameObject.SetActive(false); // replace with animation
            onClose?.Invoke();
            onRelease(this);
        }
    }
}