using System.Threading.Tasks;

namespace evm.net
{
    public interface IProvider : ILegacyProvider
    {
        Task<TR> Request<TR>(string method, object[] parameters = null);
    }
}