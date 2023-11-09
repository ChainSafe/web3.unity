using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using Scripts.EVM.Token;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;
using ABI = Scripts.EVM.Token.ABI;

public class EvmCalls : MonoBehaviour
{
    #region Fields
    
    #region IPFS

    private string apiKey = "YOUR_CHAINSAFE_STORE_API_KEY";
    private string data = "YOUR_DATA";
    private string bucketId = "BUCKET_ID";
    private string path = "/PATH";
    private string filename = "FILENAME.EXT";

    #endregion
    
    # region Contract Send
        
    private string methodSend = "addTotal";
    private string contractAddressSend = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";
    private int increaseAmountSend = 1;
        
    #endregion

    #region Contract Call

    private string methodCall = "myTotal";
    private string contractAddressCall = "0xC71d13c40B4fE7e2c557eBAa12A0400dd4Df76C9";

    #endregion

    #region Get Send Array

    private string contractAddressArray = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
    private string methodArrayGet = "getStore";
    private string methodArraySend = "setStore";
    private string[] stringArraySend =
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region Sign Verify Sha3
    
    private string messageSign = "The right man in the wrong place can make all the difference in the world.";
    private string messageSignVerify = "A man chooses, a slave obeys.";
    private string messageSha = "It’s dangerous to go alone, take this!";

    #endregion

    #region Send Transaction

    private string to = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    #endregion

    #region Registered Contract

    private string registeredContractName = "shiba";
    private string registeredContractmethod = "balanceOf";

    #endregion

    #region Private Key

    private string privateKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    private string messagePrivateKey = "This is a test message";
    private string transactionHash = "0x123456789";
    private string chainId ="5";

    #endregion

    #region Multi Call

    private string Erc20ContractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
    private string Erc20Account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion
    
    #endregion

    public async void ContractCall()
    {
        object[] args = {};
        var response = await Evm.ContractCall(Web3Accessor.Web3, methodCall, ABI.AddTotal, contractAddressCall, args);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.ContractCall));
    }

    public async void ContractSend()
    {
        object[] args =
        {
            increaseAmountSend
        };
        var response = await Evm.ContractSend(Web3Accessor.Web3, methodSend, ABI.AddTotal, contractAddressSend, args);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.ContractSend));
    }

    public async void GetArray()
    {
        var response = await Evm.GetArray(Web3Accessor.Web3, contractAddressArray, ABI.AddTotal, methodArrayGet);
        var responseString = string.Join(",\n", response.Select((list, i) => $"#{i} {string.Join((string)", ", (IEnumerable<string>)list)}"));
        SampleOutputUtil.PrintResult(responseString, nameof(Evm), nameof(Evm.GetArray));
    }
    
    public async void SendArray()
    {
        var response = await Evm.SendArray(Web3Accessor.Web3, methodArraySend, ABI.AddTotal, contractAddressArray, stringArraySend);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.SendArray));
    }
    
    public async void GetBlockNumber()
    {
        var blockNumber = await Evm.GetBlockNumber(Web3Accessor.Web3);
        SampleOutputUtil.PrintResult(blockNumber.ToString(), nameof(Evm), nameof(Evm.GetBlockNumber));
    }
    
    public async void GetGasLimit()
    {
        var gasLimit = await Evm.GetGasLimit(Web3Accessor.Web3, ABI.AddTotal, contractAddressSend, methodSend);
        SampleOutputUtil.PrintResult(gasLimit.ToString(), nameof(Evm), nameof(Evm.GetGasLimit));
    }
    
    public async void GetGasPrice()
    {
        var gasPrice = await Evm.GetGasPrice(Web3Accessor.Web3);
        SampleOutputUtil.PrintResult(gasPrice.ToString(), nameof(Evm), nameof(Evm.GetGasPrice));
    }
    
    public async void GetNonce()
    {
        var nonce = await Evm.GetNonce(Web3Accessor.Web3);
        SampleOutputUtil.PrintResult(nonce.ToString(), nameof(Evm), nameof(Evm.GetNonce));
    }
    
    public async void GetTransactionStatus()
    {
        var receipt = await Evm.GetTransactionStatus(Web3Accessor.Web3);
        var output = $"Confirmations: {receipt.Confirmations}," +
                     $" Block Number: {receipt.BlockNumber}," +
                     $" Status {receipt.Status}";

        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.GetTransactionStatus));
    }
    
    public async void RegisterContract()
    {
        var balance = await Evm.UseRegisteredContract(Web3Accessor.Web3, registeredContractName, registeredContractmethod);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Evm), nameof(Evm.UseRegisteredContract));
    }
    
    public async void SendTransaction()
    {
        var transactionHash = await Evm.SendTransaction(Web3Accessor.Web3, to);
        SampleOutputUtil.PrintResult(transactionHash, nameof(Evm), nameof(Evm.SendTransaction));
    }
    
    public void Sha3()
    {
        var hash = Evm.Sha3(messageSha);
        SampleOutputUtil.PrintResult(hash, nameof(Evm), nameof(Evm.Sha3));
    }
    
    public async void SignMessage()
    {
        var signedMessage = await Evm.SignMessage(Web3Accessor.Web3, messageSign);
        SampleOutputUtil.PrintResult(signedMessage, nameof(Evm), nameof(Evm.SignMessage));
    }
    
    public async void SignVerify()
    {
        var signatureVerified = await Evm.SignVerify(Web3Accessor.Web3, messageSignVerify);
        var output = signatureVerified ? "Verified" : "Failed to verify";
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.SignVerify));
    }
    
    public void PrivateKeySignTransaction()
    {
        var result = Evm.PrivateKeySignTransaction(privateKey, transactionHash, chainId);
        SampleOutputUtil.PrintResult(result, nameof(Evm), nameof(Evm.PrivateKeySignTransaction));
    }
    
    public void PrivateKeySignMessage()
    {
        var result = Evm.PrivateKeySignMessage(privateKey, messagePrivateKey);
        SampleOutputUtil.PrintResult(result, nameof(Evm), nameof(Evm.PrivateKeySignMessage));
    }
    
    public void PrivateKeyGetAddress()
    {
        var result = Evm.PrivateKeyGetAddress(privateKey);
        SampleOutputUtil.PrintResult(result, nameof(Evm), nameof(Evm.PrivateKeyGetAddress));
    }
    
    public async void IPFSUpload()
    {
        var cid = await Evm.Upload(new IpfsUploadRequest
        {
            ApiKey = apiKey,
            Data = data,
            BucketId = bucketId,
            Path = path,
            Filename = filename
        });
        SampleOutputUtil.PrintResult(cid, nameof(IpfsSample), nameof(IpfsSample.Upload));
    }
    
    public async void MultiCall()
    {
        var erc20Contract = Web3Accessor.Web3.ContractBuilder.Build(ABI.Erc20, Erc20ContractAddress);
        var erc20BalanceOfCalldata = erc20Contract.Calldata(CommonMethod.BalanceOf, new object[]
        {
            Erc20Account
        });
        
        var erc20TotalSupplyCalldata = erc20Contract.Calldata(CommonMethod.TotalSupply, new object[]
        {
        });

        var calls = new[]
        {
            new Call3Value()
            {
                Target = Erc20ContractAddress,
                AllowFailure = true,
                CallData = erc20BalanceOfCalldata.HexToByteArray(),
            },
            new Call3Value()
            {
                Target = Erc20ContractAddress,
                AllowFailure = true,
                CallData = erc20TotalSupplyCalldata.HexToByteArray(),
            }
        };
        
        var multicallResultResponse = await Web3Accessor.Web3.MultiCall().MultiCallAsync(calls);

        Debug.Log(multicallResultResponse);

        if (multicallResultResponse[0] != null && multicallResultResponse[0].Success)
        {
            var decodedBalanceOf = erc20Contract.Decode(CommonMethod.BalanceOf, multicallResultResponse[0].ReturnData.ToHex());
            Debug.Log($"decodedBalanceOf {((BigInteger)decodedBalanceOf[0]).ToString()}");
        }
        
        if (multicallResultResponse[1] != null && multicallResultResponse[1].Success)
        {
            var decodedTotalSupply = erc20Contract.Decode(CommonMethod.TotalSupply, multicallResultResponse[1].ReturnData.ToHex());
            Debug.Log($"decodedTotalSupply {((BigInteger)decodedTotalSupply[0]).ToString()}");
        }
    }
    
  
}
