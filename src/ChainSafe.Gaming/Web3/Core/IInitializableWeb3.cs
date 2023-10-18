using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core
{
    internal interface IInitializableWeb3
    {
        ValueTask InitializeAsync();
    }
}