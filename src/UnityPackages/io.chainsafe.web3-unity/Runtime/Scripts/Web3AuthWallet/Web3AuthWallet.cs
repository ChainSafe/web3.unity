using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Nethereum.Signer;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Scripts.Web3AuthWallet
{
    public class Web3AuthWallet
    {
        private readonly Web3AuthWalletConfig _configuration;
        private readonly IRpcProvider _provider;
        ProjectConfigScriptableObject _projectConfigSo = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));

        public Web3AuthWallet(
            IRpcProvider provider,
            Web3AuthWalletConfig configuration)
        {
            this._provider = provider;
            this._configuration = configuration;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public ValueTask Connect()
        {
            if (Connected)
            {
                throw new Web3Exception("Signer already connected.");
            }

            Connected = true;
            return default;
        }

        public Task<string> GetAddress()
        {
            if (!Connected)
            {
                throw new Web3Exception(
                    $"Can't retrieve public address. {nameof(Web3AuthWallet)} is not connected yet.");
            }

            string address = new EthECKey(_configuration?.PrivateKey).GetPublicAddress();
            return Task.FromResult(address);
        }
    }
}