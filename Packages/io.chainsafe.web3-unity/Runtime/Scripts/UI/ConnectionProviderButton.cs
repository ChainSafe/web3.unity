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
        public Button Button;

        private Action onClick;

        private void Awake()
        {
            Button.onClick.AddListener(OnClick);
        }

        public void Set(Sprite sprite, string text, Action onClick)
        {
            this.onClick = onClick;
            Icon.sprite = sprite;
            Text.text = text;
        }

        private void OnClick()
        {
            onClick?.Invoke();
        }
    }
}