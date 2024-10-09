using System;
using System.Linq;
using UnityEditor;

public static class ScriptingDefineSymbols
{
    public static bool TryAddDefineSymbol(string symbol)
    {
        PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, out string[] symbols);

        if (symbols.Contains(symbol))
        {
            return false;
        }

        symbols = symbols.Append(symbol).ToArray();
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);

        return true;
    }

    public static bool TryRemoveDefineSymbol(string symbol)
    {
        PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, out string[] symbols);

        if (!symbols.Contains(symbol))
        {
            return false;
        }

        symbols = symbols.Where(s => s != symbol).ToArray();
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);

        return true;
    }
}
