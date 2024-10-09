using System;
using ChainSafe.Gaming.Web3;
using UnityEngine;

namespace ChainSafe.Gaming
{
    [Serializable]
    public class ChainConfigEntry : IChainConfig
    {
        private const string ChainIdDefault = "11155111";
        private const string ChainDefault = "Sepolia";
        private const string NetworkDefault = "Sepolia";
        private const string SymbolDefault = "Seth";
        private const string RpcDefault = "https://rpc.sepolia.org";
        private const string BlockExplorerUrlDefault = "https://sepolia.etherscan.io";
        private const string WsDefault = "";

        [field: SerializeField] public string ChainId { get; set; }
        [field: SerializeField] public string Symbol { get; set; }
        [field: SerializeField] public string Chain { get; set; }
        [field: SerializeField] public string Network { get; set; }
        [field: SerializeField] public string Rpc { get; set; }
        [field: SerializeField] public string Ws { get; set; }
        [field: SerializeField] public string BlockExplorerUrl { get; set; }

        public static ChainConfigEntry Default => new()
        {
            ChainId = ChainIdDefault,
            Symbol = SymbolDefault,
            Chain = ChainDefault,
            Network = NetworkDefault,
            Rpc = RpcDefault,
            BlockExplorerUrl = BlockExplorerUrlDefault,
            Ws = WsDefault
        };

        public static ChainConfigEntry Empty => new()
        {
            ChainId = string.Empty,
            Symbol = string.Empty,
            Chain = "Custom",
            Network = string.Empty,
            Rpc = string.Empty,
            BlockExplorerUrl = string.Empty,
            Ws = string.Empty
        };
    }
}