using ChainSafe.GamingWeb3.Environment;
using UnityEngine;

namespace ChainSafe.GamingSdk.EVM.Unity
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