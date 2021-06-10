using UnityEngine;

public class Polygon1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet
        string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
        string tokenId = "141";

        string uri = await Polygon1155.URI(network, contract, tokenId);

        print (uri);
    }
}
