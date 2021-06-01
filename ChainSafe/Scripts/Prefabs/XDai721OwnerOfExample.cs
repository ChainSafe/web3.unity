using UnityEngine;

public class XDai721OwnerOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x90FdA259CFbdB74F1804e921F523e660bfBE698d";
        string tokenId = "1582";

        string account = await XDai721.OwnerOf(contract, tokenId);

        print (account);
    }
}
