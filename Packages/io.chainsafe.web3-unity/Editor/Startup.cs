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

                var performSync = WebGLTemplateSync.CheckSyncStatus() switch
                {
                    WebGLTemplateSyncStatus.UpToDate => false,

                    WebGLTemplateSyncStatus.DoesntExist =>
                        EditorUtility.DisplayDialog("web3.unity", "Do you wish to install the web3.unity WebGL templates into your project?", "Yes", "No"),

                    WebGLTemplateSyncStatus.OutOfDate =>
                        EditorUtility.DisplayDialog("web3.unity", "The web3.unity WebGL templates in your project are out of date, would you like to update now?", "Yes", "No"),

                    _ => false,
                };

                if (performSync)
                {
                    WebGLTemplateSync.Syncronize();
                }

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