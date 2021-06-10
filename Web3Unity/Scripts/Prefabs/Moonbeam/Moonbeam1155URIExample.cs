using UnityEngine;

public class Moonbeam1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet
        string contract = "0x6b0bc2e986B0e70DB48296619A96E9ac02c5574b";
        string tokenId = "1";

        string uri = await Moonbeam1155.URI(network, contract, tokenId);

        print (uri);
    }
}
