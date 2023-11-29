using System;
using System.Collections.Generic;
using UnityEngine;
using evm.net;
using evm.net.Models;

namespace MetaMask.Unity.Contracts
{
    public abstract class ScriptableContract<T> : ScriptableObject where T : class, IContract
    {
        [Serializable]
        public enum ChainId : int
        {
            Ethereum = 1,
            Polygon = 137,
            Bsc = 56,
            Avalanche = 43114,
            Arbitrum = 42161,
            Optimism = 10,
            zkSyncEra = 324,
            LineaTestnet = 59140,
            Goerli = 5,
        }
        
        [Serializable]
        public class AddressByChain
        {
            public ChainId ChainId;
            public string Address;
        }

        public List<AddressByChain> ContractAddresses = new List<AddressByChain>();

        private MetaMaskWallet connectedProvider;
        private Dictionary<int, T> contractInstances = new Dictionary<int, T>();

        public T CurrentContract
        {
            get
            {
                if (connectedProvider == null)
                {
                    // We need to setup
                    var success = Setup();
                    if (!success)
                        throw new InvalidOperationException("MetaMask is not currently connected");
                }

                var chainId = Convert.ToInt32(connectedProvider.SelectedChainId, 16);
                if (!contractInstances.ContainsKey(chainId))
                    throw new InvalidOperationException($"There is no contract instance setup for chainId {chainId}");

                return contractInstances[chainId];
            }
        }

        public bool HasAddressForSelectedChain
        {
            get
            {
                if (connectedProvider == null)
                {
                    // We need to setup
                    var success = Setup();
                    if (!success)
                        throw new InvalidOperationException("MetaMask is not currently connected");
                }

                var chainId = Convert.ToInt32(connectedProvider.SelectedChainId, 16);

                return contractInstances.ContainsKey(chainId);
            }
        }

        private bool Setup()
        {
            var metaMask = FindObjectOfType<MetaMaskUnity>();

            if (metaMask == null || metaMask.Wallet == null)
                return false;

            if (metaMask.Wallet.IsConnected)
            {
                SetupContract(metaMask.Wallet);
                return true;
            }
            else
            {
                metaMask.Wallet.Events.WalletAuthorized += (_, _) => SetupContract(metaMask.Wallet);
                return false;
            }
        }

        private void OnEnable()
        {
            Setup();
        }

        private void OnDisable()
        {
            contractInstances.Clear();
            connectedProvider = null;
        }

        private void OnValidate()
        {
            List<int> indexesToRemove = new List<int>();
            HashSet<ChainId> bucket = new HashSet<ChainId>();
            for (int i = 0; i < ContractAddresses.Count; i++)
            {
                if (bucket.Contains(ContractAddresses[i].ChainId))
                    indexesToRemove.Add(i);
                else
                    bucket.Add(ContractAddresses[i].ChainId);
            }

            indexesToRemove.RemoveAll(i => i == ContractAddresses.Count - 1);
            
            indexesToRemove.Reverse();
            foreach (var index in indexesToRemove)
            {
                ContractAddresses.RemoveAt(index);
            }
            
            indexesToRemove.Clear();
            bucket.Clear();
        }

        private void SetupContract(MetaMaskWallet provider)
        {
            connectedProvider = provider;
            
            foreach (var addressDetails in ContractAddresses)
            {
                var instance = Contract.Attach<T>(connectedProvider, addressDetails.Address);
                
                contractInstances.Add((int)addressDetails.ChainId, instance);
            }
        }
    }
}