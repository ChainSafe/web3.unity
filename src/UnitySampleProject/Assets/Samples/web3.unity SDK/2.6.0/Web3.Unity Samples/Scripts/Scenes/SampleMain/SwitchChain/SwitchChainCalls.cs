﻿using System;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;

namespace Samples.Behaviours.SwitchChain
{
    public class SwitchChainCalls : MonoBehaviour
    {
        public ChainSetup[] chainSetups;

        private int currentChainIndex;
        
        public async void ToggleChain()
        {
            // get next chain id
            currentChainIndex = (currentChainIndex + 1) % chainSetups.Length;
            var chainId = chainSetups[currentChainIndex].chainId;
            
            // switch chains
            await Web3Accessor.Web3.Chains.SwitchChain(chainId);
            
            Debug.Log($"Successfully switched to the chain {chainId}");
        }

        public async void CallSmartContract()
        {
            // get contract address for the current chain
            var contractAddress = chainSetups[currentChainIndex].contractAddress;
            
            // build contract client instance
            var contract = await Web3Accessor.Web3.ContractBuilder.Build<EchoChainContract>(contractAddress);
            
            // call the EchoChain function
            var echoMessage = await contract.EchoChain();
            Debug.Log(echoMessage);
        }

        public async void PrintChainId()
        {
            var chainId = await Web3Accessor.Web3.RpcProvider.GetChainId();
            Debug.Log($"Running the SDK with Chain ID: {chainId}");
        }
        
        [Serializable]
        public class ChainSetup
        {
            public string chainId;
            public string contractAddress;
        }
    }
}