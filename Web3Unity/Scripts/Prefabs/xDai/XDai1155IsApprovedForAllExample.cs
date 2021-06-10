using UnityEngine;

public class XDai1155IsApprovedForAllExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x93d0c9a35c43f6BC999416A06aaDF21E68B29EBA";
        string account = "0xa63641e81D223F01d11343C67b77CB4f092acd5a";
        string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

        bool isApproved = await XDai1155.IsApprovedForAll(contract, account, authorizedOperator);

        print (isApproved);
    }
}
