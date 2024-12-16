using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.GUI
{
    public class GuiInfoOverlay : MonoBehaviour
    {
        public TMP_Text Message;
        public TMP_Text ToastMessage;
        public GameObject ErrorIcon;
        public GameObject LoadingIcon;
        public Button CloseButton;

        public Transform Container;
        public Transform ToastContainer;
        
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

            Container.gameObject.SetActive(type != GuiOverlayType.Toast);
            ToastContainer.gameObject.SetActive(type == GuiOverlayType.Toast);
            
            ErrorIcon.SetActive(type == GuiOverlayType.Error);
            LoadingIcon.SetActive(type == GuiOverlayType.Loading);
            
            if (type != GuiOverlayType.Toast)
            {
                Message.text = message;
            }
            else
            {
                ToastMessage.text = message;
            }
        }

        public void UpdateMessage(string message)
        {
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
            // Already hidden,
            // this happens when the close button and the OnScreenClick button references are the same (tries to hide twice)
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            gameObject.SetActive(false); // replace with animation
            onClose?.Invoke();
            onRelease(this);
        }
    }
}