using System;
using System.Numerics;
using Models;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using UnityEngine;
// using Web3Unity.Scripts.Library.Web3Wallet;
using Web3Unity.Scripts.Library.ETHEREUEM.Connect;
using Web3Unity.Scripts.Library.Ethers.Transactions;

public class Mint721Voucher : MonoBehaviour
{
    // address of nft you want to mint
    public string nftAddress = "f01559ae4021a47e26bc773587278f62a833f2a6117411afbc5a7855661936d1c";

    public async void VoucherMintNft721()
    {
        var voucherResponse721 = await EVM.Get721Voucher(Web3Accessor.Web3);
        CreateRedeemVoucherModel.CreateVoucher721 voucher721 = new CreateRedeemVoucherModel.CreateVoucher721();
        voucher721.tokenId = voucherResponse721.tokenId;
        voucher721.minPrice = voucherResponse721.minPrice;
        voucher721.signer = voucherResponse721.signer;
        voucher721.receiver = voucherResponse721.receiver;
        voucher721.signature = voucherResponse721.signature;
        string voucherArgs = JsonUtility.ToJson(voucher721);

        // connects to user's browser wallet to call a transaction
        var chainConfig = Web3Accessor.Web3.ChainConfig;
        RedeemVoucherTxModel.Response voucherResponse = await EVM.CreateRedeemTransaction(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network, voucherArgs, "721", nftAddress, voucherResponse721.receiver);
        var txRequest = new TransactionRequest
        {
            ChainId = HexBigIntUtil.ParseHexBigInt(chainConfig.ChainId),
            To = voucherResponse.tx.to,
            Value = new HexBigInteger(voucherResponse.tx.value),
            Data = voucherResponse.tx.data,
            GasLimit = HexBigIntUtil.ParseHexBigInt(voucherResponse.tx.gasLimit),
            GasPrice = HexBigIntUtil.ParseHexBigInt(voucherResponse.tx.gasPrice),
        };
        var response = await Web3Accessor.Web3.TransactionExecutor.SendTransaction(txRequest);
        Debug.Log(JsonConvert.SerializeObject(response));
    }
}
