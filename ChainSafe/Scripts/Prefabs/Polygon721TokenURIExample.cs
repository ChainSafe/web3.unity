using System.Numerics;
using UnityEngine;

public class Polygon721TokenURIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet
        string contract = "0xbCCaa7ACb552A2c7eb27C7eb77c2CC99580735b9";
        string tokenId = "965";

        string uri = await Polygon721.TokenURI(network, contract, tokenId);

        print (uri);
    }
}
