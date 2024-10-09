using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3.Core.Logout;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Data used to restore a connection provider from a previous session.
    /// </summary>
    public class StoredConnectionProviderData : IStorable, IWeb3InitializedHandler, ILogoutHandler
    {
        private ILocalStorage _localStorage;

        [JsonIgnore]
        public string StoragePath => "connection-provider-data.json";

        [JsonIgnore]
        public bool LoadOnInitialize => true;

        /// <summary>
        /// Full Type Name of connection provider to be restored.
        /// </summary>
        [JsonProperty]
        public string TypeName { get; set; }

        /// <summary>
        /// Type of connection provider to be restored.
        /// </summary>
        [JsonIgnore]
        public Type Type => string.IsNullOrEmpty(TypeName) ? null : Type.GetType(TypeName);

        public async Task OnWeb3Initialized(CWeb3 web3)
        {
            _localStorage = web3.ServiceProvider.GetService<ILocalStorage>();

            await _localStorage.Save(this);
        }

        public Task OnLogout()
        {
            _localStorage.Clear(this);

            return Task.CompletedTask;
        }
    }
}
