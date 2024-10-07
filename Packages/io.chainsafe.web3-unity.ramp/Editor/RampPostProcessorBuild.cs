#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampProcessBuild : IPreprocessBuildWithReport
    {
        private const string RAMP_AVAILABLE = "RAMP_AVAILABLE";

        //Use it as the last callback order so everything else gets to do it first.
        public int callbackOrder => int.MaxValue;

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_WEBGL || UNITY_IOS
            ScriptingDefineSymbols.TryAddDefineSymbol(RAMP_AVAILABLE);
#endif
        }
    }
}
#endif