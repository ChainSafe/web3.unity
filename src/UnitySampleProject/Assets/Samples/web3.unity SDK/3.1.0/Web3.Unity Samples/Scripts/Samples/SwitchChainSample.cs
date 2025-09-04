using System;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.Evm.Contracts.Custom;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;

namespace Samples.Behaviours.SwitchChain
{
    public class SwitchChainSample : MonoBehaviour, ISample
    {
        [field: SerializeField] public string Title { get; private set; }

        [field: SerializeField, TextArea] public string Description { get; private set; }

        public Type[] DependentServiceTypes => Array.Empty<Type>();

        public ChainSetup[] chainSetups;

        private int _currentChainIndex;

        public async Task<string> ToggleChain()
        {
            // get next chain id
            var previousChainIndex = _currentChainIndex;
            _currentChainIndex = (_currentChainIndex + 1) % chainSetups.Length;
            var chainId = chainSetups[_currentChainIndex].chainId;

            Debug.Log("Switching the chain... Make sure you confirm the chain change in your wallet.");

            try
            {
                await Web3Unity.Web3.Chains.SwitchChain(chainId);

                return $"Chain Switched from {previousChainIndex} to {_currentChainIndex}";
            }
            catch
            {
                _currentChainIndex = previousChainIndex; // revert chain index toggling
                throw;
            }
        }

        public async Task<string> CallSmartContract()
        {
            // get contract address for the current chain
            var contractAddress = chainSetups[_currentChainIndex].contractAddress;

            // build contract client instance
            var contract = await Web3Unity.Web3.ContractBuilder.Build<EchoChainContract>(contractAddress);

            // call the EchoChain function
            return await contract.EchoChain();
        }

        public async Task<string> PrintChainId()
        {
            var chainId = await Web3Unity.Web3.RpcProvider.GetChainId();
            return $"Running the SDK with Chain ID: {chainId}";
        }

        /// <summary>
        /// Native ERC20 balance of an Address
        /// </summary>
        public async Task<string> NativeBalanceOf()
        {
            var balance = await Web3Unity.Web3.RpcProvider.GetBalance(Web3Unity.Instance.PublicAddress);

            return balance.ToString();
        }

        [Serializable]
        public struct ChainSetup
        {
            public string chainId;
            public string contractAddress;
        }
    }
}