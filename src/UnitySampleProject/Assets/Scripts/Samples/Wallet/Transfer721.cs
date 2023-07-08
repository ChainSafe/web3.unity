using UnityEngine;

public class Transfer721 : MonoBehaviour
{
    public async void OnTransfer721()
    {
        var contractAddress = "0x31A61D3B956d9E95e0b9434BEf24bfEebB48b2c5";
        var abi = ABI.ERC_721;
        var method = EthMethod.SafeTransferFrom;
        var account = await Web3Accessor.Web3.Signer.GetAddress();
        var toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        var tokenId = "0";
        var contract = Web3Accessor.Web3.ContractBuilder.Build(abi, contractAddress);
        var response = contract.Send(method, new object[]
         {
             account,
             toAccount,
             tokenId
         });
        Debug.Log(response);
    }
}