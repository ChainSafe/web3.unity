using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "binance";
        string network = "mainnet";
        string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
        string tokenId = "770";

        string uri = await ERC1155.URI(chain, network, contract, tokenId);
        print(uri);
    }
}
