using System.Numerics;
using Nethereum.ABI.EIP712;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Web3.Core.Evm
{
    /// <summary>
    /// A variant of Nethereum's domain which has set JSON deserialization parameters.
    /// </summary>
    [Struct("EIP712Domain")]
    public class SerializableDomain : IDomain
    {
        [Parameter("string", "name", 1)]
        [JsonProperty(PropertyName = "name")]
        public virtual string Name { get; set; }

        [Parameter("string", "version", 2)]
        [JsonProperty(PropertyName = "version")]
        public virtual string Version { get; set; }

        [Parameter("uint256", "chainId", 3)]
        [JsonProperty(PropertyName = "chainId")]
        public virtual BigInteger? ChainId { get; set; }

        [Parameter("address", "verifyingContract", 4)]
        [JsonProperty(PropertyName = "verifyingContract")]
        public virtual string VerifyingContract { get; set; }
    }
}