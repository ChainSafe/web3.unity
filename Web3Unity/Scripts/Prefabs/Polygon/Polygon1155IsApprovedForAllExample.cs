using UnityEngine;

public class Polygon1155IsApprovedForAllExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet 
        string contract = "0xfd1dBD4114550A867cA46049C346B6cD452ec919";
        string account = "0x72b8Df71072E38E8548F9565A322B04b9C752932";
        string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

        // Queries the approval status of an operator for a given owner
        bool isApproved = await Polygon1155.IsApprovedForAll(network, contract, account, authorizedOperator);

        print (isApproved);
    }
}
