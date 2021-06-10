using UnityEngine;

public class BEP1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet 
        string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
        string tokenId = "770";

        string uri = await BEP1155.URI(network, contract, tokenId);

        print (uri);
    }
}
