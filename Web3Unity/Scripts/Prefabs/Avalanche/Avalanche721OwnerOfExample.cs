using System.Numerics;
using UnityEngine;

public class Avalanche721OwnerOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet
        string contract = "0xcB0cbcE06860f6C30C62560f5eFBF918150e056E";
        string tokenId = "2";

        string account = await Avalanche721.OwnerOf(network, contract, tokenId);

        print (account);
    }
}
