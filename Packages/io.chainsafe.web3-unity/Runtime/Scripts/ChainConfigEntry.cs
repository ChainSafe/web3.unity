using System;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core.Chains;
using UnityEngine;

namespace ChainSafe.Gaming
{
    [Serializable]
    public class ChainConfigEntry : IChainConfig
    {
        private const string ChainIdDefault = "11155111";
        private const string ChainDefault = "Sepolia";
        private const string NetworkDefault = "Sepolia";

        private static readonly NativeCurrencyUnityWrapper DefaultNativeCurrency = new()
        {
            Name = "Sepolia Ether",
            Symbol = "ETH",
            Decimals = 18
        };

        private const string RpcDefault = "https://rpc.sepolia.org";
        private const string BlockExplorerUrlDefault = "https://sepolia.etherscan.io";
        private const string WsDefault = "";

        [field: SerializeField] public string ChainId { get; set; }

        public INativeCurrency NativeCurrency
        {
            get => nativeCurrency;
            set => nativeCurrency = new NativeCurrencyUnityWrapper()
            {
                Name = value.Name,
                Symbol = value.Symbol,
                Decimals = value.Decimals
            };
        }

        [SerializeField] private NativeCurrencyUnityWrapper nativeCurrency ;
        [field: SerializeField] public string Chain { get; set; }
        [field: SerializeField] public string Network { get; set; }
        [field: SerializeField] public string Rpc { get; set; }
        [field: SerializeField] public string Ws { get; set; }
        [field: SerializeField] public string BlockExplorerUrl { get; set; }
        [field: SerializeField] public string ViemName { get; set; }

        public static ChainConfigEntry Default => new()
        {
            ChainId = ChainIdDefault,
            NativeCurrency = DefaultNativeCurrency,
            Chain = ChainDefault,
            Network = NetworkDefault,
            Rpc = RpcDefault,
            BlockExplorerUrl = BlockExplorerUrlDefault,
            Ws = WsDefault
        };

        public static ChainConfigEntry Empty => new()
        {
            ChainId = string.Empty,
            NativeCurrency = new NativeCurrencyUnityWrapper(),
            Chain = "Custom",
            Network = string.Empty,
            Rpc = string.Empty,
            BlockExplorerUrl = string.Empty,
            Ws = string.Empty
        };
    }

    /// <summary>
    /// This is basically a wrapper for the INativeCurrency interface to allow for Unity serialization.
    /// </summary>
    [Serializable]
    public class NativeCurrencyUnityWrapper : INativeCurrency
    {
        [field: SerializeField] public string Name { get; set; } = "Ether";
        [field: SerializeField] public string Symbol { get; set; } = "ETH";
        [field: SerializeField] public int Decimals { get; set; } = 18;
    }
}