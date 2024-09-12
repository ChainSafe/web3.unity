#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampProcessBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private const string RAMP_AVAILABLE = "RAMP_AVAILABLE";

        //Use it as the last callback order so everything else gets to do it first.
        public int callbackOrder => int.MaxValue;

        private static string _previousSymbols;

        public void OnPostprocessBuild(BuildReport report)
        {
#if UNITY_WEBGL || UNITY_IOS
            RemoveDefineSymbol(RAMP_AVAILABLE);
#endif
        }

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_WEBGL || UNITY_IOS
            AddDefineSymbol(RAMP_AVAILABLE);
#endif
        }

        private static void AddDefineSymbol(string symbol)
        {
            _previousSymbols =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!_previousSymbols.Contains(symbol))
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                    _previousSymbols + ";" + symbol);
        }

        private static void RemoveDefineSymbol(string symbol)
        {
            var symbols =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (symbols.Contains(symbol))
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                    _previousSymbols);
        }
    }
}
#endif