using ChainSafe.Gaming.Web3;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    [CreateAssetMenu(fileName = "ProjectConfigData", menuName = "ScriptableObjects/ProjectConfigScriptableObject",
        order = 1)]
    public class ProjectConfigScriptableObject : ScriptableObject, ICompleteProjectConfig
    {
        [SerializeField] private string projectID;
        [SerializeField] private string chainID;
        [SerializeField] private string chain;
        [SerializeField] private string network;
        [SerializeField] private string symbol;
        [SerializeField] private string rpc;

        public string Symbol
        {
            get => symbol;
            set => symbol = value;
        }

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

        public string Ipc
        {
            get => rpc;
            set => rpc = value;
        }

        public string Ws
        {
            get => rpc;
            set => rpc = value;
        }
    }
}