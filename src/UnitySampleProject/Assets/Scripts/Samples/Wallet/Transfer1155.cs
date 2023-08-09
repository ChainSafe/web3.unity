using UnityEngine;

public class Transfer1155 : MonoBehaviour
{
    public async void Start()
    {
        var account = await Web3Accessor.Web3.Signer.GetAddress();
        var toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        var contractAddress = "0xae283E79a5361CF1077bf2638a1A953c872AD973";
        var abi = ABI.ERC_1155;
        var method = EthMethod.SafeTransferFrom;
        var tokenId = 0;
        var amount = 1;
        byte[] dataObject = { };

        var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, contractAddress);
        var response = await contract.Send(method, new object[]
        {
            account,
            toAccount,
            tokenId,
            amount,
            dataObject
        });
        Debug.Log("Transfer Length: " + response.Length);
        Debug.Log("Transfer Rank: " + response.Rank);
    }
}