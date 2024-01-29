using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.Common
{
    public class ErrorPopup : MonoBehaviour
    {
        public Text MessageLabel;

        /// <summary>
        /// Display error pop up when Login fails.
        /// </summary>
        /// <param name="message">Error message to be displayed.</param>
        public void ShowError(string message)
        {
            gameObject.SetActive(true);
            MessageLabel.text = message;
        }

        /// <summary>
        /// Close error popup.
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}