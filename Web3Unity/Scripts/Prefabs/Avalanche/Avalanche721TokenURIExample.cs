using System.Numerics;
using UnityEngine;

public class Avalanche721TokenURIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet
        string contract = "0xcB0cbcE06860f6C30C62560f5eFBF918150e056E";
        string tokenId = "1";

        string uri = await Avalanche721.TokenURI(network, contract, tokenId);

        print (uri);
    }
}
