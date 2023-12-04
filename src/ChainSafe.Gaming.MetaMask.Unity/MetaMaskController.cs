using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Hex.HexTypes;
using Nethereum.Unity.Metamask;
using UnityEngine;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public class MetaMaskController : MonoBehaviour
    {
        private bool isInitialized;

        private ILogWriter logger;

        public delegate void AccountConnected(string address);

        public event AccountConnected OnAccountConnected;

        public void Initialize(ILogWriter logWriter)
        {
            logger = logWriter;
        }

        public Task<string> Connect()
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            OnAccountConnected -= Connected;

            OnAccountConnected += Connected;

            void Connected(string address)
            {
                if (!taskCompletionSource.TrySetResult(address))
                {
                    logger.LogError("Error setting connected account address.");
                }
            }

            if (MetamaskWebglInterop.IsMetamaskAvailable())
            {
                MetamaskWebglInterop.EnableEthereum(gameObject.name, nameof(EthereumEnabled), nameof(DisplayError));
            }
            else
            {
                DisplayError("Metamask is not available, please install it.");

                // Unsubscribe to event.
                OnAccountConnected -= Connected;

                return null;
            }

            return taskCompletionSource.Task;
        }

        public void EthereumEnabled(string address)
        {
            logger.Log("Ethereum Enabled.");

            if (!isInitialized)
            {
                MetamaskWebglInterop.EthereumInit(gameObject.name, nameof(NewAccountSelected), nameof(ChainChanged));
                MetamaskWebglInterop.GetChainId(gameObject.name, nameof(ChainChanged), nameof(DisplayError));

                isInitialized = true;
            }

            InvokeAccountConnected(address);
        }

        private void InvokeAccountConnected(string address)
        {
            OnAccountConnected?.Invoke(address);
        }

        public void NewAccountSelected(string address)
        {
            logger.Log("New Account Selected.");
        }

        public void ChainChanged(string chainId)
        {
            logger.Log($"Selected Chain Id {new HexBigInteger(chainId).Value}.");
        }

        public void DisplayError(string message)
        {
            logger.LogError(message);
        }
    }
}