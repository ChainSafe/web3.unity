#if ENABLE_VSTU

#if UNITY_ANDROID
using System.IO;
using UnityEngine;
#endif

#if !UNITY_2017
using UnityEditor.Build.Reporting;
#endif
using UnityEditor;
using UnityEditor.Build;

namespace Plugins.CountlySDK.Editor
{

#if UNITY_2017
    internal class CountlyBuildProcessor : IPreprocessBuild
    {
        
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
#else
    internal class CountlyBuildProcessor : IPreprocessBuildWithReport
    {
        public void OnPreprocessBuild(BuildReport report)
        {
#endif

#if UNITY_ANDROID
            string directoryPath = "/Plugins/Android/Notifications/";
            string filePath = "/Plugins/Android/Notifications/libs/countly_notifications.jar";
            if (!File.Exists(Application.dataPath + "" + filePath)) {
                if (Directory.Exists(Application.dataPath + directoryPath) && !File.Exists(Application.dataPath + "" + filePath)) {
                    Debug.LogError("[CountlyBuildProcessor] notifications.jar not found at: " + filePath);
                }
            }
#endif
        }
        public int callbackOrder { get { return 0; } }
    }


}
#endif