#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class MarketplacePostProcessBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    private const string MARKETPLACE_AVAILABLE = "MARKETPLACE_AVAILABLE";
    private static string _previousSymbols;
    
    public int callbackOrder => int.MaxValue;
    

    public void OnPostprocessBuild(BuildReport report)
    {
    #if UNITY_WEBGL || UNITY_IOS
            RemoveDefineSymbol(MARKETPLACE_AVAILABLE);
    #endif
    }

    public void OnPreprocessBuild(BuildReport report)
    {
    #if UNITY_WEBGL || UNITY_IOS
            AddDefineSymbol(MARKETPLACE_AVAILABLE);
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
#endif