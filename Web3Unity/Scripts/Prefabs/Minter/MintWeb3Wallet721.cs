using Models;
using UnityEngine;

public class MintWeb3Wallet721 : MonoBehaviour
{
    // set chain: ethereum, moonbeam, polygon etc
    public string chain = "ethereum";
    // chain id
    public string chainId = "5";
    // set network mainnet, testnet
    public string network = "goerli";
    // address of nft you want to mint
    public string nftAddress = "f01559ae4021a47e26bc773587278f62a833f2a6117411afbc5a7855661936d1c";
    // type
    string type = "721";

    public async void VoucherMintNft721()
    {
        var voucherResponse721 = await EVM.Get721Voucher();
        CreateRedeemVoucherModel.CreateVoucher721 voucher721 = new CreateRedeemVoucherModel.CreateVoucher721();
        voucher721.tokenId = voucherResponse721.tokenId;
        voucher721.minPrice = voucherResponse721.minPrice;
        voucher721.signer = voucherResponse721.signer;
        voucher721.receiver = voucherResponse721.receiver;
        voucher721.signature = voucherResponse721.signature;
        string voucherArgs = JsonUtility.ToJson(voucher721);

        // connects to user's browser wallet to call a transaction
        RedeemVoucherTxModel.Response voucherResponse = await EVM.CreateRedeemTransaction(chain, network, voucherArgs, type, nftAddress, voucherResponse721.receiver);
        string response = await Web3Wallet.SendTransaction(chainId, voucherResponse.tx.to, voucherResponse.tx.value.ToString(), voucherResponse.tx.data, voucherResponse.tx.gasLimit, voucherResponse.tx.gasPrice);
        print("Response: " + response);
    }
}
