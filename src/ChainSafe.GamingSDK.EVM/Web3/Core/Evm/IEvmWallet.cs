using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Evm.Signers;

namespace ChainSafe.GamingWeb3.Evm
{
  public interface IEvmWallet : IEvmSigner
  {
    bool Connected { get; }
    ValueTask Connect();
  }
}