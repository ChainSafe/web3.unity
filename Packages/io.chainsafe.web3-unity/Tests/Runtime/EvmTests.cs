using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using Scripts.EVM.Token;
using Tests.Runtime;
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
        var callContract = Evm.ContractCall(web3, ContractCallMethod, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, args);
        yield return new WaitUntil(() => callContract.IsCompleted);
        if (callContract.Exception != null) throw callContract.Exception;
        Assert.IsTrue(callContract.IsCompletedSuccessfully);
        Assert.AreEqual(CallAmount, callContract.Result[0].ToString());
    }

    [UnityTest]
    public IEnumerator TestGetArray()
    {
        var getArray = Evm.GetArray<string>(web3, ChainSafeContracts.ArrayTotal, ABI.ArrayTotal, GetArrayMethod);
        yield return new WaitUntil(() => getArray.IsCompleted);
        // Convert toLower to make comparing easier
        var result = getArray.Result.ConvertAll(a => a.ConvertAll(b => b.ToLower()));
        Assert.AreEqual(result, new List<List<string>>
        {
            ArrayToSend.ConvertAll(a => a.ToLower())
        });
    }

    [UnityTest]
    public IEnumerator TestGetBlockNumber()
    {
        var getBlockNumber = Evm.GetBlockNumber(web3);
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
        var getGasLimit = Evm.GetGasLimit(web3, ABI.ArrayTotal, ChainSafeContracts.ArrayTotal, ContractSendMethod, args);
        yield return new WaitUntil(() => getGasLimit.IsCompleted);
        if (getGasLimit.Exception != null) throw getGasLimit.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasLimit.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestGetGasPrice()
    {
        var getGasPrice = Evm.GetGasPrice(web3);
        yield return new WaitUntil(() => getGasPrice.IsCompleted);
        if (getGasPrice.Exception != null) throw getGasPrice.Exception;
        // Just assert successful completion because result is always changing
        Assert.IsTrue(getGasPrice.IsCompletedSuccessfully);
    }

    [UnityTest]
    public IEnumerator TestUseRegisteredContract()
    {
        var useRegisteredContract = Evm.UseRegisteredContract(web3, "CsTestErc20", EthMethods.BalanceOf);
        yield return new WaitUntil(() => useRegisteredContract.IsCompleted);
        if (useRegisteredContract.Exception != null) throw useRegisteredContract.Exception;
        Assert.IsTrue(useRegisteredContract.IsCompletedSuccessfully);
        Assert.AreEqual(new BigInteger(999999999999999), useRegisteredContract.Result);
    }

    [UnityTest]
    public IEnumerator TestSha3()
    {
        var sha3 = Evm.Sha3("It’s dangerous to go alone, take this!");
        Assert.AreEqual("45760485edafabcbb28570e7da5ddda4639abb4794de9af6de1d6669cbf220fc", sha3);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestECDSASignTx()
    {
        var signTxECDSA = Evm.EcdsaSignTransaction(ecdsaKey, transactionHash, chainId);
        Assert.AreEqual("0xf8cd1c5dfa0706767c7ec81b91bed9a05b65b9c5b47769206a594377c5969a9a47fe21f7328b5865815f97b7ddd09bd7b899879aac6ed7c01eda5380975a54dc01546d72", signTxECDSA);
        yield break;
    }

    [UnityTest]
    public IEnumerator TestECDSASign()
    {
        var signECDSA = Evm.EcdsaSignMessage(ecdsaKey, ecdsaMessage);
        Assert.AreEqual("0x90bd386a185bdc5cbb13b9bba442e35036ac8e92792e74e385abbf7d9546be8720879c2075bf59124d9114b4d432173a4d6c4b118d278c3ffb51a140a625c66c1b", signECDSA);
        yield break;
    }

    [UnityTest]
    public IEnumerator TestECDSAAddress()
    {
        var address = Evm.EcdsaGetAddress(ecdsaKey);
        Assert.AreEqual("0x428066dd8A212104Bc9240dCe3cdeA3D3A0f7979", address);
        yield break;
    }

    [UnityTest]
    public IEnumerator TestCustomBalanceOfErc20()
    {
        var getCustomBalanceOf = web3.Erc20.GetBalanceOf(ChainSafeContracts.Erc20);
        yield return new WaitUntil(() => getCustomBalanceOf.IsCompleted);
        Assert.AreEqual(new BigInteger(999999999999999), getCustomBalanceOf.Result);
    }
}
