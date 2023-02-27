using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ProjectConfigData", menuName = "ScriptableObjects/ProjectConfigScriptableObject",
    order = 1)]
public class ProjectConfigScriptableObject : ScriptableObject
{
    [SerializeField] public string projectID;
    [SerializeField] public string chainID;
    [SerializeField] public string chain;
    [SerializeField] public string network;
    [SerializeField] public string rpc;

    public string ProjectID
    {
        get => projectID;
        set => projectID = value;
    }

    public string ChainID
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

    public string RPC
    {
        get => rpc;
        set => rpc = value;
    }
}