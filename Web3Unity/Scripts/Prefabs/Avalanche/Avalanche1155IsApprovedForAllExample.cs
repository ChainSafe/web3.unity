using UnityEngine;

public class Avalanche1155IsApprovedForAllExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // mainnet testnet 
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";
        string account = "0x72b8Df71072E38E8548F9565A322B04b9C752932";
        string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

        // Queries the approval status of an operator for a given owner
        bool isApproved = await Avalanche1155.IsApprovedForAll(network, contract, account, authorizedOperator);

        print (isApproved);
    }
}
