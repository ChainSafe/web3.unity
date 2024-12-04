
using UnityEngine;

namespace Plugins.CountlySDK.Helpers
{
    internal class Constants
    {
        public const string SdkVersion = "24.8.0";

#if UNITY_EDITOR
        public const string SdkName = "csharp-unity-editor";
#elif UNITY_ANDROID
        public const string SdkName = "csharp-unity-android";
#elif UNITY_IOS
         public const string SdkName = "csharp-unity-ios";
#elif UNITY_STANDALONE_WIN
         public const string SdkName = "csharp-unity-windows";
#elif UNITY_STANDALONE_OSX
         public const string SdkName = "csharp-unity-osx";
#elif UNITY_STANDALONE_LINUX
         public const string SdkName = "csharp-unity-linux";
#elif UNITY_WEBGL
         public const string SdkName = "csharp-unity-webgl";
#elif UNITY_TVOS
         public const string SdkName = "csharp-unity-tvos";
#elif UNITY_WSA_10_0
         public const string SdkName = "csharp-unity-uwp";
#elif UNITY_PS4
         public const string SdkName = "csharp-unity-ps4"
#elif UNITY_XBOXONE
         public const string SdkName = "csharp-unity-xboxone"
#else
         public const string SdkName = "generic";
#endif

        public const string CountlyServerUrl = "https://us-try.count.ly/";
        public const string DeviceIDKey = "DeviceID";
        public const string DeviceIDTypeKey = "DeviceIDType";


        public const string SchemaVersion = "Countly.SchemaVersion";
        public const string FirstAppLaunch = "Countly.FirstAppLaunch";
        public const string FirstAppLaunchSegment = "firstAppLaunch";

        #region Notification Keys

        public const string MessageIDKey = "c.i";
        public const string TitleDataKey = "title";
        public const string MessageDataKey = "message";
        public const string ImageUrlKey = "c.m";
        public const string ActionButtonKey = "c.b";
        public const string SoundDataKey = "sound";

        #endregion
    }
}
