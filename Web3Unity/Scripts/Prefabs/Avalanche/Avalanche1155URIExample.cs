using UnityEngine;

public class Avalanche1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";
        string tokenId = "1";

        string uri = await Avalanche1155.URI(network, contract, tokenId);

        print (uri);
    }
}
