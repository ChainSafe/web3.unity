using UnityEngine;

public class BEP1155IsApprovedForAllExample : MonoBehaviour
{
    async void Start()
    {
        string network = "mainnet"; // mainnet testnet
        string contract = "0x3E31F70912c00AEa971A8b2045bd568D738C31Dc";
        string account = "0xe91e3b8b25f41b215645813a33e39edf42ba25cf";
        string authorizedOperator = "0x35706484aB20Cbf22F5c7a375D5764DA8166aE1c";

        // Queries the approval status of an operator for a given owner
        bool isApproved = await BEP1155.IsApprovedForAll(network, contract, account, authorizedOperator);

        print (isApproved);
    }
}
