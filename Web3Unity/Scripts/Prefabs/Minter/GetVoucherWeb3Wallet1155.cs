using System;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class GetVoucherWeb3Wallet1155 : MonoBehaviour
{
    public async void Get1155VoucherButton()
    {
        var voucherResponse1155 = await EVM.Get1155Voucher();
        Debug.Log("Voucher Response 1155 Signature: " + voucherResponse1155.signature);
        Debug.Log("Voucher Response 1155 Min Price: " + voucherResponse1155.minPrice);
        Debug.Log("Voucher Response 1155 Token ID: " + voucherResponse1155.tokenId);
        Debug.Log("Voucher Response 1155 Nonce: " + voucherResponse1155.nonce);
        Debug.Log("Voucher Response 1155 Signer: " + voucherResponse1155.signer);
        Debug.Log("Voucher Response 1155 Amount: " + voucherResponse1155.amount);
        // saves the voucher to player prefs, you can change this if you like to fit your system
        PlayerPrefs.SetString("Web3Voucher1155Sig", voucherResponse1155.signature);
        PlayerPrefs.SetString("Web3Voucher1155TokenID", voucherResponse1155.tokenId);
        PlayerPrefs.SetString("Web3Voucher1155Signer", voucherResponse1155.signer);
        PlayerPrefs.SetString("Web3Voucher1155Nonce", voucherResponse1155.nonce.ToString());
        PlayerPrefs.SetString("Web3Voucher1155Amount", voucherResponse1155.amount);
        PlayerPrefs.SetString("Web3Voucher1155MinPrice", voucherResponse1155.minPrice.ToString());
    }
}