using UnityEngine;

public class Moonbeam721OwnerOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet
        string contract = "0xcB0cbcE06860f6C30C62560f5eFBF918150e056E";
        string tokenId = "1";

        string account = await Moonbeam721.OwnerOf(network, contract, tokenId);

        print (account);
    }
}
