#if UNITY_WEBGL && UNITY_EDITOR

using UnityEditor;


namespace ChainSafe.Gaming.Reown.AppKit
{
    /// <summary>
    /// Used to define the APPKIT_AVAILABLE preprocessor directive when the AppKit package is installed.
    /// </summary>
    [InitializeOnLoad]
    public class AppKitPreprocessorDefine
    {
        private const string AppkitAvailable = "APPKIT_AVAILABLE";
        private const string AppKitInstalled = "AppKitInstalled";
        static AppKitPreprocessorDefine()
        {
            if (SessionState.GetBool(AppKitInstalled, false))
                return;
            
            ScriptingDefineSymbols.TryAddDefineSymbol(AppkitAvailable);
            SessionState.SetBool(AppKitInstalled, true);
        }
    }
}
#endif