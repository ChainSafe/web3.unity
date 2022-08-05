using System;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class GetVoucherWeb3Wallet721 : MonoBehaviour
{
    public async void Get721VoucherButton()
    {
        var voucherResponse721 = await EVM.Get721Voucher();
        Debug.Log("Voucher Response 721 Signature : " + voucherResponse721.signature);
        Debug.Log("Voucher Response 721 Uri : " + voucherResponse721.uri);
        Debug.Log("Voucher Response 721 Signer : " + voucherResponse721.signer);
        Debug.Log("Voucher Response 721 Min Price : " + voucherResponse721.minPrice);
        // saves the voucher to player prefs, you can change this if you like to fit your system
        PlayerPrefs.SetString("Web3Voucher721", voucherResponse721.signer);
    }
}
