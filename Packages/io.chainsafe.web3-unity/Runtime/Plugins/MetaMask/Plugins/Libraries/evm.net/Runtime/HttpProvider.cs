using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using evm.net.Models;
using evm.net.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace evm.net
{
    public class HttpProvider : IProvider
    {
        private static readonly JsonConverter[] Converters = {
            new BigIntegerHexConverter()
        };

        private IHttpService _httpService;
        private long _httpChain;
        
        private readonly Dictionary<long, string> rpcUrlMap;
        private readonly ILegacyProvider connectedProvider;
        private readonly string authHeader;
        private readonly string authKey;

        public HttpProvider(Dictionary<long, string> rpcUrlMap, ILegacyProvider provider, string authHeader = null, string authKey = null)
        {
            this.rpcUrlMap = rpcUrlMap;
            this.connectedProvider = provider;
            this.authHeader = authHeader;
            this.authKey = authKey;
        }

        public long ChainId
        {
            get
            {
                return connectedProvider.ChainId;
            }
        }

        public string ConnectedAddress
        {
            get
            {
                return connectedProvider.ConnectedAddress;
            }
        }

        public Task<TR> Request<TR>(string method, object[] parameters = null)
        {
            return RpcRequest<TR>(method, parameters);
        }

        public object Request(string method, object[] parameters = null)
        {
            return RawRpcRequest(method, parameters);
        }

        private async Task<TR> RpcRequest<TR>(string method, object[] parameters = null)
        {
            SetupHttpService();
            var request = new JsonRpcRequest(method, parameters);
            var requestJson = JsonConvert.SerializeObject(request);
            Debug.Log("Sending HTTP Request: " + requestJson);
            var json = await this._httpService.Post("", requestJson);

            var payload = JsonConvert.DeserializeObject<JsonRpcPayload>(json);

            if (payload.IsError)
            {
                var error = JsonConvert.DeserializeObject<JsonRpcError>(json);

                if (error?.Error == null)
                {
                    throw new IOException($"RPC returned error: {json}");
                }

                throw new IOException($"RPC returned error: {error.Error.Code} - {error.Error.Message}");
            }

            Debug.Log("Got HTTP Response: " + json);
            
            return JsonConvert.DeserializeObject<JsonRpcResult<TR>>(json, Converters).Result;
        }
        
        private object RawRpcRequest(string method, object[] parameters = null)
        {
            SetupHttpService();
            var request = new JsonRpcRequest(method, parameters);
            var json = this._httpService.Post("", JsonConvert.SerializeObject(request));

            json.Wait();

            return JsonConvert.DeserializeObject<JsonRpcPayload>(json.Result, Converters);
        }

        private void SetupHttpService()
        {
            var selectedChain = ChainId;
            
            if (_httpService != null && selectedChain == _httpChain)
                return;

            if (!rpcUrlMap.ContainsKey(selectedChain))
                throw new IOException($"No RPC url set for chain id {selectedChain}");

            var rpcUrl = rpcUrlMap[selectedChain];
            this._httpService = HttpServiceFactory.NewHttpService(rpcUrl, authKey, authHeader);
            _httpChain = selectedChain;
        }
    }
}