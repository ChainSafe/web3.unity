using UnityEngine;

public class Transfer721 : MonoBehaviour
{
    public async void Start()
    {
        var contractAddress = "0x358AA13c52544ECCEF6B0ADD0f801012ADAD5eE3";
        var abi = ABI.ERC_721;
        var method = EthMethod.SafeTransferFrom;
        var account = await Web3Accessor.Web3.Signer.GetAddress();
        var toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        var tokenId = "0";
        var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, contractAddress);
        var response = await contract.Send(method, new object[]
         {
             account,
             toAccount,
             tokenId
         });
        Debug.Log("Transfer Length: " + response.Length);
        Debug.Log("Transfer Rank: " + response.Rank);
    }
}