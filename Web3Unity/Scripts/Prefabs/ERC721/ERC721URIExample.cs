using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC721URIExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "polygon";
        string network = "mainnet";
        string contract = "0xbCCaa7ACb552A2c7eb27C7eb77c2CC99580735b9";
        string tokenId = "965";

        string uri = await ERC721.URI(chain, network, contract, tokenId);
        print(uri);
    }
}
