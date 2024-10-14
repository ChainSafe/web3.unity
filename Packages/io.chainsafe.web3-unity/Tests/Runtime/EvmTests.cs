using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using NUnit.Framework;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.TestTools;

public class EvmTests : SampleTestsBase
{
    #region Fields

    #region ContractCalls

    private const string ContractSendMethod = "addTotal";
    private const int IncreaseAmount = 2;
    private const string ContractCallMethod = "myTotal";
    private const string CallAmount = "1";

    #endregion

    #region Array

    private const string GetArrayMethod = "getStore";

    private static List<string> ArrayToSend = new()
    {
        "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
        "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
    };

    #endregion

    #region ECDSA

    private string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    private string ecdsaMessage = "This is a test message";
    private string transactionHash = "0x123456789";
    private string chainId = "11155111";

    #endregion

    #endregion

    [UnityTest]
    public IEnumerator TestContractCall()
    {
        var address = web3.Signer.PublicAddress;
        object[] args = { address };
        var callContract = Web3Unity.Instance.ContractCall(ContractCallMethod, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);
        yield return new WaitUntil(() => callContract.IsCompleted);
        if (callContract.Exception != null) throw callContract.Exception;
        Assert.IsTrue(callContract.IsCompletedSuccessfully);
        Assert.AreEqual(CallAmount, callContract.Result[0].ToString());
    }

    [UnityTest]
    public IEnumerator TestGetBlockNumber()
    {
        var getBlockNumber = Web3Unity.Instance.GetBlockNumber();
        yield return new WaitUntil(() => getBlockNumber.IsCompleted);
        if (getBlockNumber.Exception != null) throw getBlockNumber.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getBlockNumber.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasLimit()
    {
        object[] args =
        {
           IncreaseAmount
        };
        var getGasLimit = Web3Unity.Instance.GetGasLimit(ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, ContractSendMethod, args);
        yield return new WaitUntil(() => getGasLimit.IsCompleted);
        if (getGasLimit.Exception != null) throw getGasLimit.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasLimit.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasPrice()
    {
        var getGasPrice = Web3Unity.Instance.GetGasPrice();
        yield return new WaitUntil(() => getGasPrice.IsCompleted);
        if (getGasPrice.Exception != null) throw getGasPrice.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasPrice.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator GetMessageHash()
    {
        var sha3 = Web3Unity.Instance.GetMessageHash("It’s dangerous to go alone, take this!");
        Assert.AreEqual("45760485edafabcbb28570e7da5ddda4639abb4794de9af6de1d6669cbf220fc", sha3);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SignMessageWithPrivateKey()
    {
        var signature = Web3Unity.Instance.SignMessageWithPrivateKey(ecdsaKey, ecdsaMessage);
        Assert.AreEqual("0x90bd386a185bdc5cbb13b9bba442e35036ac8e92792e74e385abbf7d9546be8720879c2075bf59124d9114b4d432173a4d6c4b118d278c3ffb51a140a625c66c1b", signature);
        yield break;
    }

    [UnityTest]
    public IEnumerator GetPublicAddressFromPrivateKey()
    {
        var address = Web3Unity.Instance.GetPublicAddressFromPrivateKey(ecdsaKey);
        Assert.AreEqual("0x428066dd8A212104Bc9240dCe3cdeA3D3A0f7979", address);
        yield break;
    }

    [UnityTest]
    public IEnumerator TestCustomBalanceOfErc20()
    {
        var getCustomBalanceOf = web3.Erc20.GetBalanceOf(ChainSafeContracts.Erc20);
        yield return new WaitUntil(() => getCustomBalanceOf.IsCompleted);
        Assert.AreEqual(BigInteger.Parse("24997000000000000000"), getCustomBalanceOf.Result);
    }
}
