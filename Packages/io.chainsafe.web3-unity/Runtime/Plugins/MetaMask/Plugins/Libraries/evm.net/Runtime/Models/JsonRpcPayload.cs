using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace evm.net.Models
{
    public class JsonRpcPayload : JsonRpcBase
    {
        [JsonExtensionData]
#pragma warning disable CS0649
        private IDictionary<string, JToken> _more;
#pragma warning restore CS0649
        
        /// <summary>
        /// Get the method for this request payload
        /// </summary>
        [JsonIgnore]
        public string Method
        {
            get
            {
                if (!IsRequest)
                    throw new ArgumentException("The given payload is not a request, and thus has no Method");

                return _more["method"].ToObject<string>();
            }
        }
        
        /// <summary>
        /// Returns true if this payload represents a request
        /// </summary>+
        [JsonIgnore]
        public bool IsRequest
        {
            get
            {
                return _more != null && _more.ContainsKey("method");
            }
        }

        /// <summary>
        /// Returns true if this payload represents a response
        /// </summary>
        [JsonIgnore]
        public bool IsResponse
        {
            get
            {
                return _more != null && _more.ContainsKey("result") || IsError;
            }
        }

        /// <summary>
        /// Returns true if this payload represents an error
        /// </summary>
        [JsonIgnore]
        public bool IsError
        {
            get
            {
                return _more != null && _more.ContainsKey("error");
            }
        }
    }
}