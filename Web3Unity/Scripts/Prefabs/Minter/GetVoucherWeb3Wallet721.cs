using System;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class GetVoucherWeb3Wallet721 : MonoBehaviour
{
    public async void Get721VoucherButton()
    {
        var voucherResponse = await EVM.Get721Voucher("https://lazy-minting-voucher-signer.herokuapp.com");
        Debug.Log("Voucher Response: " + voucherResponse);
        print(voucherResponse);
    }
}
