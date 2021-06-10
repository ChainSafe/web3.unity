using System.Numerics;
using UnityEngine;

public class XDai721TokenURIExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x90FdA259CFbdB74F1804e921F523e660bfBE698d";
        string tokenId = "1582";

        string uri = await XDai721.TokenURI(contract, tokenId);

        print (uri);
    }
}
