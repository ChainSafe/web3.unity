using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core.Logout
{
    public interface ILogoutHandler
    {
        Task OnLogout();
    }
}