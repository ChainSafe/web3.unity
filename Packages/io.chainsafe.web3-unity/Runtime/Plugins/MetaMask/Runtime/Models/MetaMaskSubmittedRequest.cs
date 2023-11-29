using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace MetaMask.Models
{
    public class MetaMaskSubmittedRequest
    {

        [JsonProperty("method")]
        [JsonPropertyName("method")]
        public string Method { get; set; }

    }
}
