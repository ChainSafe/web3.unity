using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC721OwnerOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "mainnet";
        string contract = "0xA74E199990FF572A320508547Ab7f44EA51e6F28";
        string[] tokenIds = {"700", "791"};
        string multicall = ""; // optional: multicall contract https://github.com/makerdao/multicall
        string rpc = ""; // optional: custom rpc

        List<string> batchOwners = await ERC721.OwnerOfBatch(chain, network, contract, tokenIds, multicall, rpc);
        foreach (string owner in batchOwners)
        {
            print ("OwnerOfBatch: " + owner);
        } 
    }
}
