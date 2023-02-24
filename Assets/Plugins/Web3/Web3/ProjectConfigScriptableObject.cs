using UnityEngine;

[CreateAssetMenu(fileName = "ProjectConfigData", menuName = "ScriptableObjects/ProjectConfigScriptableObject", order = 1)]
public class ProjectConfigScriptableObject : ScriptableObject
{
    public string ProjectID;
    public string ChainID;
    public string Chain;
    public string Network;
    public string RPC;
}