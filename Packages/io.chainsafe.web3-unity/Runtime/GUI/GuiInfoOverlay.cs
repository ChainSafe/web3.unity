using TMPro;
using UnityEngine;

namespace ChainSafe.Gaming.GUI
{
    public class GuiInfoOverlay : MonoBehaviour
    {
        public TMP_Text Message;
        public GameObject ErrorIcon;
        public GameObject LoadingIcon;
        private bool deactivateOnClick;

        public void Initialize(GuiOverlayType type, string message, bool deactivateOnClick)
        {
            this.deactivateOnClick = deactivateOnClick;
            ErrorIcon.SetActive(type == GuiOverlayType.Error);
            LoadingIcon.SetActive(type == GuiOverlayType.Loading);
            Message.text = message;
        }

        public void OnScreenClick()
        {
            if (!deactivateOnClick)
            {
                return;
            }
            
            gameObject.SetActive(false);
        }
    }
}