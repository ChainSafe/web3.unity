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

                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return Platform.IOS;
                    case RuntimePlatform.Android:
                        return Platform.Android;
                    case RuntimePlatform.WebGLPlayer:
                        return Platform.WebGL;
                    default:
                        return Platform.Desktop;
                }
            }
        }

        public void OpenUrl(string url) => Application.OpenURL(url);
    }
}