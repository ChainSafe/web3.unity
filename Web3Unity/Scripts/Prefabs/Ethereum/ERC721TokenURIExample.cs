using System.Numerics;
using UnityEngine;

public class ERC721TokenURIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0xf5b0a3efb8e8e4c201e2a935f110eaaf3ffecb8d";
        string tokenId = "721";

        string uri = await ERC721.TokenURI(network, contract, tokenId);

        print (uri);
    }
}
