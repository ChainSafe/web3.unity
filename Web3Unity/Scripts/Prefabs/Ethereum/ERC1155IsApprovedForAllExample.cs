using UnityEngine;

public class ERC1155IsApprovedForAllExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
        string account = "0xaca9b6d9b1636d99156bb12825c75de1e5a58870";
        string authorizedOperator = "0x3482549fca7511267c9ef7089507c0f16ea1dcc1";

        // Queries the approval status of an operator for a given owner
        bool isApproved = await ERC1155.IsApprovedForAll(network, contract, account, authorizedOperator);

        print (isApproved);
    }
}
