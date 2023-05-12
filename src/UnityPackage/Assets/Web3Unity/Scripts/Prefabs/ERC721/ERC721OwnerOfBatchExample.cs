using System.Collections.Generic;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC721OwnerOfBatchExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x47381c5c948254e6e0E324F1AA54b7B24104D92D";
        string[] tokenIds = { "33", "29" };
        string multicall = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e"; // optional: multicall contract https://github.com/makerdao/multicall

        List<string> batchOwners = await ERC721.OwnerOfBatch(contract, tokenIds, multicall);
        foreach (string owner in batchOwners)
        {
            print("OwnerOfBatch: " + owner);
        }
    }
}
