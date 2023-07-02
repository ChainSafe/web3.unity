using System.Threading.Tasks;
using UnityEngine;

namespace Prefabs.Web3AuthWallet.Interfaces
{
    public interface IHttpRequestHandler
    {
        Task<T> PostRequest<T>(string url, WWWForm form);
    }
}
