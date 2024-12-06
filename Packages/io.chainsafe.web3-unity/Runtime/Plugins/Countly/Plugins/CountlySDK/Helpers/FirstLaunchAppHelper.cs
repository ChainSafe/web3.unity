using System;
using UnityEngine;

namespace Plugins.CountlySDK.Helpers
{
    /// <summary>
    /// Helper class for managing and tracking the first launch of the application.
    /// </summary>
    internal static class FirstLaunchAppHelper
    {
        private static bool? _firstLaunchApp;

        /// <summary>
        /// Processes the first launch of the application.
        /// 
        /// <para>
        /// This method checks if the application has been launched for the first time.
        /// If it is the first launch, it sets the appropriate flag and saves it to PlayerPrefs.
        /// </para>
        /// </summary>
        public static void Process()
        {
            if (!PlayerPrefs.HasKey(Constants.FirstAppLaunch)) {
                PlayerPrefs.SetInt(Constants.FirstAppLaunch, 1);
                PlayerPrefs.Save();
                _firstLaunchApp = true;
            } else {
                PlayerPrefs.SetInt(Constants.FirstAppLaunch, 0);
                PlayerPrefs.Save();
                _firstLaunchApp = false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is the first launch of the application.                   
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this is the first launch of the application; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFirstLaunchApp
        {
            get {
                if (!_firstLaunchApp.HasValue) {
                    Debug.LogWarning("[FirstLaunchAppHelper] IsFirstLaunchApp : Process should be called when session begins");
                    Process();
                }
                return _firstLaunchApp.Value;
            }
        }
    }
}
