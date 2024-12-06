#if ENABLE_VSTU
using UnityEditor;
using SyntaxTree.VisualStudio.Unity.Bridge;

/// <summary>
/// This class hooks into the Visual Studio .sln generation step and modifies the file
/// to include .editorconfig, which enforces consistent formatting standards and naming
/// conventions.
/// https://docs.microsoft.com/en-us/visualstudio/ide/editorconfig-code-style-settings-reference?view=vs-2017
/// </summary>
[InitializeOnLoad]
public class EditorConfigSolutionFileGenerator
{
    public static string kEditorConfigProjectFindStr = "EndProject\r\nGlobal";
    public static string kEditorConfigProjectReplaceStr =
        "EndProject\r\n" +
        "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Solution Items\", \"Solution Items\", \"{B24FE069-BB5F-4F16-BCDA-61C28EABC46B}\"\r\n" +
        "	ProjectSection(SolutionItems) = preProject\r\n" +
        "		.editorconfig = .editorconfig\r\n" +
        "	EndProjectSection\r\n" +
        "EndProject\r\n" +
        "Global";

    public static string kGlobalSectionFindStr = "EndGlobalSection\r\nEndGlobal";
    public static string kGlobalSectionReplaceStr =
        "EndGlobalSection\r\n" +
        "	GlobalSection(ExtensibilityGlobals) = postSolution\r\n" +
        "		SolutionGuid = {FD87994B-C032-4821-BD72-E057C33083EF}\r\n" +
        "	EndGlobalSection\r\n" +
        "EndGlobal";

    static EditorConfigSolutionFileGenerator()
    {
        ProjectFilesGenerator.SolutionFileGeneration += AppendEditorConfig;
    }

    protected static string AppendEditorConfig(string fileName, string fileContent)
    {
        fileContent = fileContent.Replace(kEditorConfigProjectFindStr, kEditorConfigProjectReplaceStr);
        fileContent = fileContent.Replace(kGlobalSectionFindStr, kGlobalSectionReplaceStr);

        return fileContent;
    }
}
#endif
