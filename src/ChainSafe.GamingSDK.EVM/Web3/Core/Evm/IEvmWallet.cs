using System.Threading.Tasks;

namespace ChainSafe.GamingWeb3.Evm
{
  public interface IEvmWallet
  {
    bool Connected { get; }
    ValueTask Connect();
  }
}