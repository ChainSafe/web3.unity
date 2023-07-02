using Nethereum.Signer;
using Prefabs.Web3AuthWallet.Interfaces;

namespace Prefabs.Web3AuthWallet.Utils
{
    public class EthereumService : IEthereumService
    {
        public string GetAddressW3A(string privateKey) => new EthECKey(privateKey).GetPublicAddress();
    }
}
