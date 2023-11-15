using UnityEngine;
using UnityEditor;
using System;
using ChainSafe.Gaming.UnityPackage;
using PlasticPipe.PlasticProtocol.Messages;

namespace ChainSafe.GamingSdk.Editor
{
    [InitializeOnLoad]
    public class Startup : EditorWindow
    {
        static bool initialized = false;

        static Startup()
        {
            EditorApplication.update += () =>
            {
                if (initialized)
                {
                    return;
                }

                initialized = true;

                if (EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    // Don't want these checks to happen when starting play mode
                    return;
                }
                
                // Checks project ID
                ValidateProjectID();
            };
        }

        static void ValidateProjectID()
        {
            try
            {
                var projectID = ProjectConfigUtilities.Load()?.ProjectId;
                if (string.IsNullOrEmpty(projectID))
                {
                    ChainSafeServerSettings.ShowWindow();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to validate project ID");
                Debug.LogException(e);
            }
        }
    }
}