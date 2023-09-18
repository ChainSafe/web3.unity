using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using UnityEngine;
using Scripts.EVM.Remote;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

public class Mint1155 : MonoBehaviour
{
    // address of nft you want to mint
    public string nftAddress = "0x2c1867bc3026178a47a677513746dcc6822a137a";

    public async void VoucherMintNft1155()
    {
        var voucherResponse1155 = await CSServer.Get1155Voucher(Web3Accessor.Web3);
        CreateRedeemVoucherModel.CreateVoucher1155 voucher1155 = new CreateRedeemVoucherModel.CreateVoucher1155();
        voucher1155.tokenId = voucherResponse1155.tokenId;
        voucher1155.minPrice = voucherResponse1155.minPrice;
        voucher1155.signer = voucherResponse1155.signer;
        voucher1155.receiver = voucherResponse1155.receiver;
        voucher1155.amount = voucherResponse1155.amount;
        voucher1155.nonce = voucherResponse1155.nonce;
        voucher1155.signature = voucherResponse1155.signature;
        string voucherArgs = JsonUtility.ToJson(voucher1155);

        // connects to user's browser wallet to call a transaction
        var chainConfig = Web3Accessor.Web3.ChainConfig;
        RedeemVoucherTxModel.Response voucherResponse = await CSServer.CreateRedeemTransaction(Web3Accessor.Web3, chainConfig.Chain, chainConfig.Network, voucherArgs, "1155", nftAddress, voucherResponse1155.receiver);
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
