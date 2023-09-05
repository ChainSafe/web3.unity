using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public class ErrorPopup : MonoBehaviour
    {
        public Text MessageLabel;

        public void ShowError(string message)
        {
            gameObject.SetActive(true);
            MessageLabel.text = message;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}