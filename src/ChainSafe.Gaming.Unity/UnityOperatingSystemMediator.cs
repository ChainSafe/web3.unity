using System;
using ChainSafe.Gaming.Web3.Environment;
using UnityEngine;

namespace ChainSafe.Gaming.Unity
{
    public class UnityOperatingSystemMediator : IOperatingSystemMediator
    {
        public bool IsMobilePlatform => Application.isMobilePlatform;

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

        public void OpenUrl(string url) => Application.OpenURL(url);
    }
}