#if UNITY_WEBGL
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class WebGLResourceEmbedding : IPreprocessBuildWithReport
{

    public int callbackOrder => 1;
    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
}
#endif