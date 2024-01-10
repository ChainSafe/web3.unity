using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IConnectionDialogProvider
    {
        Task<IConnectionDialog> ProvideDialog();
    }
}