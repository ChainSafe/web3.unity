#if UNITY_WEBGL
using ChainSafe.Gaming.UnityPackage.Connection;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ChainSafe.Gaming.Editor.Reown
{
    public class ReownWebGLPreBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            Resources.Load<ReownConnectionProvider>("ReownConnectionProvider").PopulateViemNames(true);
        }
    }
}
#endif