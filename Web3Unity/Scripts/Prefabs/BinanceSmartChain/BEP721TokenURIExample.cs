using System.Numerics;
using UnityEngine;

public class BEP721TokenURIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet
        string contract = "0x3e855B7941fE8ef5F07DAd68C5140D6a3EC1b286";
        string tokenId = "1008";

        string uri = await BEP721.TokenURI(network, contract, tokenId);

        print (uri);
    }
}
