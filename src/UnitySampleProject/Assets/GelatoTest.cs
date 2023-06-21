using System;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using ChainSafe.GamingSdk.Gelato;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Contract = Web3Unity.Scripts.Library.Ethers.Contracts.Contract;

public class GelatoTest : MonoBehaviour
{
    private Web3 _web3;

    // Start is called before the first frame update
    async void Start()
    {
        await InitWeb3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task InitWeb3()
    {
        var sponsorApiKey = System.Environment.GetEnvironmentVariable("1BalanceApiKey");
        
        _web3 = await new Web3Builder(ProjectConfigUtilities.Load()).Configure(services =>
        {
            services.UseUnityEnvironment();
            services.UseJsonRpcProvider();
            services.UseWebPageWallet();
            services.UseGelatoModule(sponsorApiKey);
        }).BuildAsync();
    }

    async Task CallWithSyncFeeExample()
    {
        var vitalik = "0xd8da6bf26964af9d7eed9e03e53415d37aa96045";
        var target = "0xA045eb75e78f4988d42c3cd201365bDD5D76D406";
        var feeToken = "0xEeeeeEeeeEeEeeEeEeEeeEEEeeeeEeeeeeeeEEeE";
        var abi = "[{\"inputs\": [" +
                  "{\"internalType\":\"address\",\"name\":\"_token\",\"type\":\"address\"}," +
                  "{\"internalType\":\"address\",\"name\":\"_to\",\"type\":\"address\"}," +
                  "{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}" +
                  "]," +
                  "\"name\":\"sendToFriend\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";
        var contract = new Contract(abi, target);
        var data = contract.Calldata("sendToFriend", new object[]
        {
            feeToken,
            vitalik,
            0.0005m
        });
        var gelatoInstance = _web3.ServiceProvider.GetRequiredService<IGelatoModule>();
        var relayResponse = await gelatoInstance.CallWithSyncFee(new CallWithSyncFeeRequest()
        {
            ChainId = new HexBigInteger(5),
            Target = target,
            Data = data,
            FeeToken = feeToken,
            IsRelayContext = true,
        });

        var complete = false;
        while (!complete)
        {
            var status = await gelatoInstance.GetTaskStatus(relayResponse.TaskId);
            Debug.Log($"CallWithSyncFee status: {relayResponse.TaskId}: {status.TaskState}");

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    Debug.Log("Task complete");
                    complete = true;
                    Debug.Log($"Final status of {relayResponse.TaskId}: {status.TaskState}");
                    Debug.Log($"Transaction hash: {status.TransactionHash}: ");
                    break;
                default:
                    Thread.Sleep(2000);
                    continue;
            }
        }
    }
    
    async Task SponsorCallExample()
    {
        var counterContract = "0x30d97B13e29B0cd42e6ebd48dbD9063465bF1997";
        
        var abi = "[{\"inputs\": []," +
                  "\"name\":\"incrementContext\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";
        var contract = new Contract(abi, counterContract);
        var data = contract.Calldata("incrementContext");
        
        var gelatoInstance = _web3.ServiceProvider.GetRequiredService<IGelatoModule>();
        var relayResponse = await gelatoInstance.SponsoredCall(new SponsoredCallRequest()
        {
            ChainId = new HexBigInteger(5),
            Target = counterContract,
            Data = data,
        });

        var complete = false;
        while (!complete)
        {
            var status = await gelatoInstance.GetTaskStatus(relayResponse.TaskId);
            Debug.Log($"SponsorCall status: {relayResponse.TaskId}: {status.TaskState}");

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    Debug.Log("Task complete");
                    complete = true;
                    Debug.Log($"Final status of {relayResponse.TaskId}: {status.TaskState}");
                    Debug.Log($"Transaction hash: {status.TransactionHash}: ");
                    break;
                default:
                    Thread.Sleep(2000);
                    continue;
            }
        }
    }
    
    async Task CallWithSyncFeeErc2771Example()
    {
        var vitalik = "0xd8da6bf26964af9d7eed9e03e53415d37aa96045";
        var target = "0xA045eb75e78f4988d42c3cd201365bDD5D76D406";
        var feeToken = "0xEeeeeEeeeEeEeeEeEeEeeEEEeeeeEeeeeeeeEEeE";
        var abi = "[{\"inputs\": [" +
                  "{\"internalType\":\"address\",\"name\":\"_token\",\"type\":\"address\"}," +
                  "{\"internalType\":\"address\",\"name\":\"_to\",\"type\":\"address\"}," +
                  "{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}" +
                  "]," +
                  "\"name\":\"sendToFriend\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";
        var contract = new Contract(abi, target);
        var data = contract.Calldata("sendToFriend", new object[]
        {
            feeToken,
            vitalik,
            0.0005m
        });

        var gelatoInstance = _web3.ServiceProvider.GetRequiredService<IGelatoModule>();
        var relayResponse = await gelatoInstance.CallWithSyncFeeErc2771(new CallWithSyncFeeErc2771Request()
        {
            ChainId = new HexBigInteger(5),
            Target = target,
            Data = data,
            FeeToken = feeToken,
            IsRelayContext = true,
        }, _web3.Signer);

        var complete = false;
        while (!complete)
        {
            var status = await gelatoInstance.GetTaskStatus(relayResponse.TaskId);
            Debug.Log($"CallWithSyncFeeErc2771 status: {relayResponse.TaskId}: {status.TaskState}");

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    Debug.Log("Task complete");
                    complete = true;
                    Debug.Log($"Final status of {relayResponse.TaskId}: {status.TaskState}");
                    Debug.Log($"Transaction hash: {status.TransactionHash}: ");
                    break;
                default:
                    Thread.Sleep(2000);
                    continue;
            }
        }
    }
    
    async Task sponsorCallErc2771Example()
    {
        var counterContract = "0x30d97B13e29B0cd42e6ebd48dbD9063465bF1997";
        
        var abi = "[{\"inputs\": []," +
                  "\"name\":\"incrementContext\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";
        var contract = new Contract(abi, counterContract);
        var data = contract.Calldata("incrementContext");
        
        var gelatoInstance = _web3.ServiceProvider.GetRequiredService<IGelatoModule>();
        var relayResponse = await gelatoInstance.SponsoredCallErc2771(new SponsoredCallErc2771Request()
        {
            Target = counterContract,
            Data = data,
            User = await _web3.Signer.GetAddress(),
        }, _web3.Signer);

        var complete = false;
        while (!complete)
        {
            var status = await gelatoInstance.GetTaskStatus(relayResponse.TaskId);
            Debug.Log($"SponsorCallErc2771 status: {relayResponse.TaskId}: {status.TaskState}");

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    Debug.Log("Task complete");
                    complete = true;
                    Debug.Log($"Final status of {relayResponse.TaskId}: {status.TaskState}");
                    Debug.Log($"Transaction hash: {status.TransactionHash}: ");
                    break;
                default:
                    Thread.Sleep(2000);
                    continue;
            }
        }
    }

}
