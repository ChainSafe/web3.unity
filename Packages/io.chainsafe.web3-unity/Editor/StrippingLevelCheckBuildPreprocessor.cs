using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class StrippingLevelCheckBuildPreprocessor : IPreprocessBuildWithReport
{
    private const string dialogMessage = "Some of the components of the ChainSafe Gaming SDK might not behave properly when stripping level is stricter than Minimal.\n\nDo you want to switch the stripping level setting to Minimal?";
    private const string dialogTitle = "Set Stripping Level to Minimal";

    public int callbackOrder { get; }

    public void OnPreprocessBuild(BuildReport report)
    {
        var buildGroup = report.summary.platformGroup;

        var strippingLevel = PlayerSettings.GetManagedStrippingLevel(buildGroup);

        if (strippingLevel == ManagedStrippingLevel.Disabled) return;
        if (strippingLevel == ManagedStrippingLevel.Minimal) return;

        var switchToMinimal = EditorUtility.DisplayDialog(
            dialogTitle,
            dialogMessage,
            "Switch to Minimal",
            "Ignore");

        if (!switchToMinimal) return;

        PlayerSettings.SetManagedStrippingLevel(buildGroup, ManagedStrippingLevel.Minimal);
    }
}