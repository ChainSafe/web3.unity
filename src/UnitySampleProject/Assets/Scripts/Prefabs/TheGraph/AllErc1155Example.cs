using UnityEngine;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;

public class AllErc1155Example : MonoBehaviour
{
    private class NFTs
    {
        public string contract { get; set; }
        public string tokenId { get; set; }
        public string uri { get; set; }
        public string balance { get; set; }
    }

    private async void Start()
    {
        var chain = "ethereum";
        var network = "goerli"; // mainnet goerli
        var account = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";
        var contract = "";
        var first = 1000;
        var skip = 0;
        var response = await EVM.AllErc1155(chain, network, account, contract, first, skip);
        try
        {
            var erc1155s = JsonConvert.DeserializeObject<NFTs[]>(response);
            print(erc1155s[0].contract);
            print(erc1155s[0].tokenId);
            print(erc1155s[0].uri);
            print(erc1155s[0].balance);
        }
        catch
        {
            print("Error: " + response);
        }
    }
}