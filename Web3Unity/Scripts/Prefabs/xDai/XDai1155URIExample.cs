using UnityEngine;

public class XDai1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
        string tokenId = "1344";

        string uri = await XDai1155.URI(contract, tokenId);

        print (uri);
    }
}
