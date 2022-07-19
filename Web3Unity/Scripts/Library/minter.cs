using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minter : MonoBehaviour
{
    async void Start()
    {
        var voucherResponse721 = await EVM.Get721Voucher();
        Debug.Log("Voucher Response 721 Signature : " + voucherResponse721.signature);
        Debug.Log("Voucher Response 721 Uri : " + voucherResponse721.uri);
        Debug.Log("Voucher Response 721 Signer : " + voucherResponse721.signer);
        Debug.Log("Voucher Response 721 Min Price : " + voucherResponse721.minPrice);


        var voucherResponse1155 = await EVM.Get1155Voucher();
        Debug.Log("Response Data: " + voucherResponse1155);
        Debug.Log("Voucher Response 1155 Signature: " + voucherResponse1155.signature);
        Debug.Log("Voucher Response 1155 Min Price: " + voucherResponse1155.minPrice);
        Debug.Log("Voucher Response 1155 Token ID: " + voucherResponse1155.tokenId);
        Debug.Log("Voucher Response 1155 Nonce: " + voucherResponse1155.nonce);
        Debug.Log("Voucher Response 1155 Signer: " + voucherResponse1155.signer);
        Debug.Log("Voucher Response 1155 Amount: " + voucherResponse1155.amount);
    }
}
