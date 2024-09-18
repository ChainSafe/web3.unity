using UnityEditor;
using System.IO;
using UnityEngine;

[InitializeOnLoad]
public class AddMarketplaceScriptingDefineSymbol
{
    private const string DefineSymbol = "MARKETPLACE_AVAILABLE";
    private static bool initialized;

    static AddMarketplaceScriptingDefineSymbol()
    {
        ScriptingDefineSymbols.TryAddDefineSymbol(DefineSymbol);
    }
    
}