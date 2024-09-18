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
            var previousChainIndex = currentChainIndex;
            currentChainIndex = (currentChainIndex + 1) % chainSetups.Length;
            var chainId = chainSetups[currentChainIndex].chainId;
            
            Debug.Log("Switching the chain... Make sure you confirm the chain change in your wallet.");
            
            // switch chains
            SampleFeedback.Instance.Activate();
            try
            {
                await Web3Unity.Web3.Chains.SwitchChain(chainId);
            }
            catch
            {
                currentChainIndex = previousChainIndex; // revert chain index toggling
                throw;
            }
            finally
            {
                SampleFeedback.Instance.Deactivate();
            }
        }

        public async void CallSmartContract()
        {
            // get contract address for the current chain
            var contractAddress = chainSetups[currentChainIndex].contractAddress;
            
            // build contract client instance
            var contract = await Web3Unity.Web3.ContractBuilder.Build<EchoChainContract>(contractAddress);
            
            // call the EchoChain function
            var echoMessage = await contract.EchoChain();
            Debug.Log(echoMessage);
        }

        public async void PrintChainId()
        {
            var chainId = await Web3Unity.Web3.RpcProvider.GetChainId();
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