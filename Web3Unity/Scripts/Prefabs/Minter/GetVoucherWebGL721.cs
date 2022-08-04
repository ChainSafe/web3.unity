using System;
using Models;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_WEBGL
public class GetVoucherWebGL721 : MonoBehaviour
{
    public async void Get721VoucherButton()
    {
        var voucherResponse721 = await EVM.Get721Voucher();
        Debug.Log("Voucher Response 721 Signature : " + voucherResponse721.signature);
        Debug.Log("Voucher Response 721 Uri : " + voucherResponse721.uri);
        Debug.Log("Voucher Response 721 Signer : " + voucherResponse721.signer);
        Debug.Log("Voucher Response 721 Min Price : " + voucherResponse721.minPrice);
        // saves the voucher to player prefs, you can change this if you like to fit your system
        PlayerPrefs.SetString("WebGLVoucher721Sig", voucherResponse721.signature);
        PlayerPrefs.SetString("WebGLVoucher721URI", voucherResponse721.uri);
        PlayerPrefs.SetString("WebGLVoucher721Signer", voucherResponse721.signer);
        PlayerPrefs.SetString("WebGLVoucher721MinPrice", voucherResponse721.minPrice.ToString());
    }
}
#endif