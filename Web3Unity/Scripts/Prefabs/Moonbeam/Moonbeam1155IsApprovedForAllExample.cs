using UnityEngine;

public class Moonbeam1155IsApprovedForAllExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet
        string contract = "0x6b0bc2e986B0e70DB48296619A96E9ac02c5574b";
        string account = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

        // Queries the approval status of an operator for a given owner
        bool isApproved = await Moonbeam1155.IsApprovedForAll(network, contract, account, authorizedOperator);

        print (isApproved);
    }
}
