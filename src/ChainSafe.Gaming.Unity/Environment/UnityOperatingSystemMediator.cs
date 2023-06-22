using ChainSafe.Gaming.Environment;
using UnityEngine;

namespace ChainSafe.Gaming.Unity.Environment
{
    public class UnityOperatingSystemMediator : IOperatingSystemMediator
    {
        public string ClipboardContent
        {
            get => GUIUtility.systemCopyBuffer;
            set => GUIUtility.systemCopyBuffer = value;
        }

        public void OpenUrl(string url) => Application.OpenURL(url);
    }
}