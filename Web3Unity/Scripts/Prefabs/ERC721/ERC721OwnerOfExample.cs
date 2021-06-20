using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC721OwnerOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "moonbeam";
        string network = "testnet";
        string contract = "0xcB0cbcE06860f6C30C62560f5eFBF918150e056E";
        string tokenId = "1";

        string ownerOf = await ERC721.OwnerOf(chain, network, contract, tokenId);
        print(ownerOf);
    }
}
