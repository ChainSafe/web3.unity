using ChainSafe.Gaming.Web3;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    public class ProjectConfigScriptableObject_Deprecated : ScriptableObject
    {
        [SerializeField] private string projectID;
        [SerializeField] private string chainID;
        [SerializeField] private string chain;
        [SerializeField] private string network;
        [SerializeField] private string symbol;
        [SerializeField] private string rpc;
        [SerializeField] private string ws;
        [SerializeField] private string blockExplorerUrl;
        [SerializeField] private bool enableAnalytics;

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
            get => ws;
            set => ws = value;
        }

        public string BlockExplorerUrl
        {
            get => blockExplorerUrl;
            set => blockExplorerUrl = value;
        }

        public bool EnableAnalytics
        {
            get => enableAnalytics;
            set => enableAnalytics = value;
        }
    }
}