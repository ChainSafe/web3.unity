using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace MetaMask
{

    public class BuildPostProcessor
    {

        [PostProcessBuild(1)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.iOS)
            {
#if UNITY_IOS
                // Read.
                string projectPath = PBXProject.GetPBXProjectPath(path);
                PBXProject project = new PBXProject();
                project.ReadFromString(File.ReadAllText(projectPath));
                string projectTarget = project.GetUnityMainTargetGuid();

                // Add `-ObjC` to "Other Linker Flags".
                project.AddBuildProperty(projectTarget, "OTHER_LDFLAGS", "-ObjC");
                project.SetBuildProperty(projectTarget, "ENABLE_BITCODE", "YES");
                
                // Fix for iOS 15
                project.AddBuildProperty(projectTarget, "OTHER_CODE_SIGN_FLAGS", 
                    "--generate-entitlement-der -o runtime --runtime-version $DEPLOYMENT_TARGET");

                // Write.
                File.WriteAllText(projectPath, project.WriteToString());
#endif
            }
        }

    }

}