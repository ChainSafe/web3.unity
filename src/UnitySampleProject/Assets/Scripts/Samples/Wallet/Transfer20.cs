using UnityEngine;

public class Transfer20 : MonoBehaviour
{
    async public void Start()
    {
        string contractAddress = "0xc778417e063141139fce010982780140aa0cd5ab";
        string abi = ABI.ERC_20;
        string method = EthMethod.Transfer;
        string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        string amount = "1000000000000000";
        var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, contractAddress);
        var response = await contract.Send(method, new object[]
        {
             toAccount,
             amount
        });
        Debug.Log("Transfer Length: " + response.Length);
        Debug.Log("Transfer Rank: " + response.Rank);
    }
}
