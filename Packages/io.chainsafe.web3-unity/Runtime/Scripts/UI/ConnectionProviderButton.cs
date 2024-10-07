using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.UI
{
    public class ConnectionProviderButton : MonoBehaviour
    {
        public Image Icon;
        public TMP_Text Text;
        
        private Action onClick;

        public void Set(Sprite sprite, string text, Action onClick)
        {
            this.onClick = onClick;
            Icon.sprite = sprite;
            Text.text = text;
        }

        public void OnClick()
        {
            onClick?.Invoke();
        }
    }
}