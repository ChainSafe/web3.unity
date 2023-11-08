using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

public class EvmCalls : MonoBehaviour
{
    // Fields

    #region IPFS

    public string apiKey = "YOUR_CHAINSAFE_STORE_API_KEY";
    public string data = "YOUR_DATA";
    public string bucketId = "BUCKET_ID";
    public string path = "/PATH";
    public string filename = "FILENAME.EXT";

    #endregion
    
    # region Contract Send
        
    public string methodSend = "addTotal";
    public string abiSend = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    public string contractAddressSend = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";
    public int increaseAmountSend = 1;
        
    #endregion

    #region Contract Call

    private string methodCall = "myTotal";
    private string abiCall = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    private string contractAddressCall = "0xC71d13c40B4fE7e2c557eBAa12A0400dd4Df76C9";

    #endregion

    #region Get Send Array

    public string contractAddressArray = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
    public string abiArray = "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    public string methodArrayGet = "getStore";
    public string methodArraySend = "getStore";
    string[] stringArraySend =
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region Sign Verify Sha3
    
    public string messageSign = "The right man in the wrong place can make all the difference in the world.";
    public string messageSignVerify = "A man chooses, a slave obeys.";
    public string messageSha = "It’s dangerous to go alone, take this!";

    #endregion

    #region Send Transaction

    public string to = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    #endregion

    #region Registered Contract

    public string registeredContractName = "shiba";
    public string registeredContractmethod = "balanceOf";

    #endregion
    
    private Evm evm;
    
    // Initializes the protocol class
    public void Awake()
    {
        evm = new Evm(Web3Accessor.Web3);
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

    public async void ContractCall()
    {
        object[] args = {};
        var response = await evm.ContractCall(methodCall, abiCall, contractAddressCall, args);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.ContractCall));
    }

    public async void ContractSend()
    {
        object[] args =
        {
            increaseAmountSend
        };
        var response = await evm.ContractSend(methodSend, abiSend, contractAddressSend, args);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.ContractSend));
    }

    public async void GetArray()
    {
        var response = await evm.GetArray(contractAddressArray, abiArray, methodArrayGet);
        var responseString = string.Join(",\n", response.Select((list, i) => $"#{i} {string.Join((string)", ", (IEnumerable<string>)list)}"));
        SampleOutputUtil.PrintResult(responseString, nameof(Evm), nameof(Evm.GetArray));
    }
    
    public async void SendArray()
    {
        var response = await evm.SendArray(methodArrayGet, abiArray, contractAddressArray, stringArraySend);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.SendArray));
    }
    
    public async void GetBlockNumber()
    {
        var blockNumber = await evm.GetBlockNumber();
        SampleOutputUtil.PrintResult(blockNumber.ToString(), nameof(Evm), nameof(Evm.GetBlockNumber));
    }
    
    public async void GetGasLimit()
    {
        var gasLimit = await evm.GetGasLimit(abiSend, contractAddressSend, methodSend);
        SampleOutputUtil.PrintResult(gasLimit.ToString(), nameof(Evm), nameof(Evm.GetGasLimit));
    }
    
    public async void GetGasPrice()
    {
        var gasPrice = await evm.GetGasPrice();
        SampleOutputUtil.PrintResult(gasPrice.ToString(), nameof(Evm), nameof(Evm.GetGasPrice));
    }
    
    public async void GetNonce()
    {
        var nonce = await evm.GetNonce();
        SampleOutputUtil.PrintResult(nonce.ToString(), nameof(Evm), nameof(Evm.GetNonce));
    }
    
    public async void GetTransactionStatus()
    {
        var receipt = await evm.GetTransactionStatus();
        var output = $"Confirmations: {receipt.Confirmations}," +
                     $" Block Number: {receipt.BlockNumber}," +
                     $" Status {receipt.Status}";

        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.GetTransactionStatus));
    }
    
    public async void RegisterContract()
    {
        var balance = await evm.UseRegisteredContract(registeredContractName, registeredContractmethod);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Evm), nameof(Evm.UseRegisteredContract));
    }
    
    public async void SendTransaction()
    {
        var transactionHash = await evm.SendTransaction(to);
        SampleOutputUtil.PrintResult(transactionHash, nameof(Evm), nameof(Evm.SendTransaction));
    }
    
    public void Sha3()
    {
        var hash = evm.Sha3(messageSha);
        SampleOutputUtil.PrintResult(hash, nameof(Evm), nameof(Evm.Sha3));
    }
    
    public async void SignMessage()
    {
        var signedMessage = await evm.SignMessage(messageSign);
        SampleOutputUtil.PrintResult(signedMessage, nameof(Evm), nameof(Evm.SignMessage));
    }
    
    public async void SignVerify()
    {
        var signatureVerified = await evm.SignVerify(messageSignVerify);
        var output = signatureVerified ? "Verified" : "Failed to verify";
        SampleOutputUtil.PrintResult(output, nameof(Evm), nameof(Evm.SignVerify));
    }
}
