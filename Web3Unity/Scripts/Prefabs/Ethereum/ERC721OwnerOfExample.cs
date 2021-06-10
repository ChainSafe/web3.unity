using UnityEngine;

public class ERC721OwnerOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0xf5b0a3efb8e8e4c201e2a935f110eaaf3ffecb8d";
        string tokenId = "721";

        string account = await ERC721.OwnerOf(network, contract, tokenId);

        print (account);
    }
}
