using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3.Core.Logout;
using Newtonsoft.Json;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    public class StoredConnectionProviderData : IStorable, IWeb3InitializedHandler, ILogoutHandler
    {
        [JsonIgnore]
        public string StoragePath => "connection-provider-data.json";
        
        [JsonIgnore]
        public bool LoadOnInitialize => true;

        [JsonProperty]
        public string TypeName { get; set; }

        [JsonIgnore]
        public Type Type => string.IsNullOrEmpty(TypeName) ? null : Type.GetType(TypeName);
        
        public async Task OnWeb3Initialized(CWeb3 web3)
        {
            await this.SaveOneTime();
        }

        public Task OnLogout()
        {
            this.ClearOneTime();
            
            return Task.CompletedTask;
        }
    }
}
