using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public class WebGLProvider : IExternalProvider
    {
        public string GetPath()
        {
            return "";
        }

        public async Task<T> Send<T>(string method, object[] parameters = null)
        {
            var jsonParams = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var result = await Web3GLLight.SendAsync(method, jsonParams);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}