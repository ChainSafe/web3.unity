using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Types
{
    public class EIP712Domain
    {
        /// <summary>
        ///    DATA - name of the contract dapp eg. Cryptokitties v2
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

         /// <summary>
        ///    DATA - The current version of what the standard calls a “signing domain”. 
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string version { get; set; }
        
         /// <summary>
        ///    DATA - The EIP-155 chain id. Prevents a signature meant for one network, such as a testnet, from working on another, such as the mainnet.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public string chainId { get; set; }

         /// <summary>
        ///    DATA - The Ethereum address of the contract that will verify the resulting signature. T
        /// </summary>
        [JsonProperty(PropertyName = "verifyingContract")]
        public string verifyingContract { get; set; }
    };

    // This is the standard EIP 712 signigning ABI 
    // public  EIP712_DOMAIN_TYPE_DATA
    // {
    //   EIP712Domain: [
    //       { name: "name", type: "string" },
    //       { name: "version", type: "string" },
    //       { name: "chainId", type: "uint256" },
    //       { name: "verifyingContract", type: "address" },
    //   ],
    // };
}
