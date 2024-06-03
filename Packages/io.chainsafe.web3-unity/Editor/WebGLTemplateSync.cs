using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ChainSafe.GamingSdk.Editor
{
    static class WebGLTemplateSync
    {
        internal static WebGLTemplateSyncStatus CheckSyncStatus()
        {
            var assetsTemplatesDirectory = new DirectoryInfo(Path.Combine(Application.dataPath, "WebGLTemplates"));
            if (!assetsTemplatesDirectory.Exists)
            {
                return WebGLTemplateSyncStatus.DoesntExist;
            }

            var pluginPath = PluginPathDetector.GetPluginPath();
            var templatesFolder = new DirectoryInfo(Path.Combine(pluginPath, "Editor/WebGLTemplates"));

            return DirectoryInSync(templatesFolder, assetsTemplatesDirectory) ?
                WebGLTemplateSyncStatus.UpToDate :
                WebGLTemplateSyncStatus.OutOfDate;
        }

        private static bool DirectoryInSync(DirectoryInfo reference, DirectoryInfo check)
        {
            if (!check.Exists)
            {
                return false;
            }

            foreach (var file in reference.GetFiles())
            {
                if (!File.Exists(Path.Combine(check.FullName, file.Name)))
                {
                    return false;
                }
            }

            foreach (var directory in reference.GetDirectories())
            {
                if (!DirectoryInSync(directory, new DirectoryInfo(Path.Combine(check.FullName, directory.Name))))
                {
                    return false;
                }
            }

            return true;
        }

        [MenuItem("ChainSafe SDK/Sync WebGL Templates")]
        public static void Syncronize()
        {
            AssetDatabase.DisallowAutoRefresh();

            try
            {
                var assetsTemplatesDirectory = new DirectoryInfo(Path.Combine(Application.dataPath, "WebGLTemplates"));
                if (!assetsTemplatesDirectory.Exists)
                {
                    AssetDatabase.CreateFolder("Assets", "WebGLTemplates");
                }

                var pluginPath = PluginPathDetector.GetPluginPath();
                var templatesFolder = new DirectoryInfo(Path.Combine(pluginPath, "Editor/WebGLTemplates"));

                CopyFolder(templatesFolder, assetsTemplatesDirectory);
            }
            finally
            {
                AssetDatabase.AllowAutoRefresh();
                AssetDatabase.Refresh();
                // Update template values to chain config
                ChainSafeServerSettings.WriteNetworkFile();
            }
        }

        private static void CopyFolder(DirectoryInfo source, DirectoryInfo destination)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            foreach (var file in source.GetFiles())
            {
                if (file.Extension == ".meta")
                {
                    continue;
                }

                var destinationFile = new FileInfo(Path.Combine(destination.FullName, file.Name));
                if (destinationFile.Exists)
                {
                    destinationFile.Delete();
                }
                File.Copy(file.FullName, destinationFile.FullName);
            }

            foreach (var directory in source.GetDirectories())
            {
                CopyFolder(directory, new DirectoryInfo(Path.Combine(destination.FullName, directory.Name)));
            }
        }
    }

    enum WebGLTemplateSyncStatus
    {
        DoesntExist,
        OutOfDate,
        UpToDate,
    }
}
