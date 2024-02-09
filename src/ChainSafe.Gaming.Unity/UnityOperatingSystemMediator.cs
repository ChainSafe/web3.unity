using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;

namespace ChainSafe.Gaming.Unity
{
    public class UnityOperatingSystemMediator : IOperatingSystemMediator
    {
        public bool IsMobilePlatform => Application.isMobilePlatform;

        public bool IsEditor => Application.isEditor;

        public Platform Platform
        {
            get
            {
                if (Application.isEditor)
                {
                    return Platform.Editor;
                }

                return Application.platform switch
                {
                    RuntimePlatform.IPhonePlayer => Platform.IOS,
                    RuntimePlatform.Android => Platform.Android,
                    RuntimePlatform.WebGLPlayer => Platform.WebGL,
                    _ => Platform.Desktop,
                };
            }
        }

        public string AppPersistentDataPath => Application.persistentDataPath;

        public void OpenUrl(string url) => Application.OpenURL(url);
    }
}