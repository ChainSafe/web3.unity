using System;
using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Scripts.EVM.Token;
using TMPro;
using UnityEngine;
using WalletConnectSharp.Common.Utils;

public class SampleTransfer : MonoBehaviour
{
    [SerializeField] private TMP_Text myAddress;
    [SerializeField] private TMP_InputField amountInput;
    [SerializeField] private TMP_InputField addressInput;

    private async void Awake()
    {
        
        if (Web3Accessor.TryWeb3 == null)
        {
            throw new Web3Exception("Web3 Instance is not set. Please set it after you've configured the W");
        }
        myAddress.text = "My Address: " + await Web3Accessor.Web3.Signer.GetAddress();
    }

    public async void Transfer()
    {
        var toAddress = addressInput.text;
        var amount = amountInput.text;
        
        if(!toAddress.IsHex())
            throw new Web3Exception("Not a hex address");
    
        if (Web3Accessor.TryWeb3 == null)
        {
            throw new Web3Exception("Web3 Instance is not set. Please set it after you've built the Web3 instance.");
        }

        await Erc20.TransferErc20(Web3Accessor.Web3, Contracts.Erc20, toAddress, System.Numerics.BigInteger.Parse(amount));
    }
}
