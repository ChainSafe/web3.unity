using ChainSafe.GamingWeb3;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectConfigData", menuName = "ScriptableObjects/ProjectConfigScriptableObject",
    order = 1)]
public class ProjectConfigScriptableObject : ScriptableObject, ICompleteProjectConfig
{
    public const string DefaultAssetPath = "Assets/Resources/ProjectConfigData.asset";
    
    [SerializeField] public string projectID;
    [SerializeField] public string chainID;
    [SerializeField] public string chain;
    [SerializeField] public string network;
    [SerializeField] public string rpc;

    public string ProjectId
    {
        get => projectID;
        set => projectID = value;
    }

    public string ChainId
    {
        get => chainID;
        set => chainID = value;
    }

    public string Chain
    {
        get => chain;
        set => chain = value;
    }

    public string Network
    {
        get => network;
        set => network = value;
    }

    public string Rpc
    {
        get => rpc;
        set => rpc = value;
    }

    public static ProjectConfigScriptableObject LoadDefault() => Resources.Load<ProjectConfigScriptableObject>(DefaultAssetPath);
}