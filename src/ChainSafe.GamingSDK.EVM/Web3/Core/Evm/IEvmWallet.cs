using System.Threading.Tasks;

namespace Web3Unity.Scripts.Library.Ethers
{
  public interface IEvmWallet
  {
    bool Connected { get; }
    ValueTask Connect();
  }
}