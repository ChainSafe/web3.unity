using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    public class StoredConnectionProviderData : IStorable, IWeb3InitializedHandler
    {
        [JsonIgnore]
        public string StoragePath => "connection-provider-data.json";
        
        [JsonIgnore]
        public bool LoadOnInitialize => true;

        [JsonProperty]
        public string TypeName { get; set; }

        [JsonIgnore]
        public Type Type => string.IsNullOrEmpty(TypeName) ? null : Type.GetType(TypeName);
        
        public async Task OnWeb3Initialized()
        {
            await this.SaveOneTime();
        }
    }
}
