using UnityEngine;

public class ERC1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
        string tokenId = "17";

        string uri = await ERC1155.URI(network, contract, tokenId);

        print (uri);
    }
}
