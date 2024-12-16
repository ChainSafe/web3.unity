using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.Web3;
using ChainSafe.GamingSdk.Gelato;
using ChainSafe.GamingSdk.Gelato.Dto;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Gelato.Types;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

public class GelatoSample : ServiceAdapter, IWeb3InitializedHandler, ISample
{
    [field: SerializeField] public string Title { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    public Type[] DependentServiceTypes => new[] { typeof(IGelato) };

    private Web3 _web3;

    /// <summary>
    /// Gelato with sync fee
    /// </summary>
    public async Task<string> CallWithSyncFee()
    {
        const string vitalik = "0xd8da6bf26964af9d7eed9e03e53415d37aa96045";
        const string target = "0xA045eb75e78f4988d42c3cd201365bDD5D76D406";
        const string feeToken = "0xEeeeeEeeeEeEeeEeEeEeeEEEeeeeEeeeeeeeEEeE";
        const string abi = "[{\"inputs\": [" +
                           "{\"internalType\":\"address\",\"name\":\"_token\",\"type\":\"address\"}," +
                           "{\"internalType\":\"address\",\"name\":\"_to\",\"type\":\"address\"}," +
                           "{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}" +
                           "]," +
                           "\"name\":\"sendToFriend\"," +
                           "\"outputs\":[]," +
                           "\"stateMutability\":\"nonpayable\"," +
                           "\"type\":\"function\"" +
                           "}]";
        var contract = _web3.ContractBuilder.Build(abi, target);
        var data = contract.Calldata("sendToFriend", new object[]
        {
            feeToken,
            vitalik,
            new BigInteger(5 * 10E12),
        });

        var relayResponse = await _web3.Gelato().CallWithSyncFee(new CallWithSyncFeeRequest()
        {
            Data = data,
            FeeToken = feeToken,
            IsRelayContext = true,
            Target = target,
        });

        while (true)
        {
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    return $"Task complete. Final status of {relayResponse.TaskId}: {status.TaskState}. " +
                           $"Transaction hash: {status.TransactionHash}";
                default:
                    await WaitForSeconds(2);
                    continue;
            }
        }
    }

    /// <summary>
    /// Gelato sponsor call
    /// </summary>
    public async Task<string> SponsorCall()
    {
        const string counterContract = "0x763D37aB388C5cdd2Fb0849d6275802F959fbF30";

        const string abi = "[{\"inputs\": []," +
                           "\"name\":\"increment\"," +
                           "\"outputs\":[]," +
                           "\"stateMutability\":\"nonpayable\"," +
                           "\"type\":\"function\"" +
                           "}]";
        var contract = _web3.ContractBuilder.Build(abi, counterContract);

        var data = contract.Calldata("increment");

        var relayResponse = await _web3.Gelato().SponsoredCall(new SponsoredCallRequest()
        {
            Target = counterContract,
            Data = data,
        });

        while (true)
        {
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    return
                        $"Task complete. Final status of {relayResponse.TaskId}: {status.TaskState}. " +
                        $"Transaction hash: {status.TransactionHash}";
                default:
                    await WaitForSeconds(2);
                    continue;
            }
        }
    }

    /// <summary>
    /// Gelato2771 with sync fee
    /// </summary>
    public async Task<string> CallWithSyncFeeErc2771()
    {
        const string target = "0x5dD1100f23278e0e27972eacb4F1B81D97D071B7";
        const string feeToken = "0xEeeeeEeeeEeEeeEeEeEeeEEEeeeeEeeeeeeeEEeE";
        const string abi = "[{\"inputs\": []," +
                           "\"name\":\"increment\"," +
                           "\"outputs\":[]," +
                           "\"stateMutability\":\"nonpayable\"," +
                           "\"type\":\"function\"" +
                           "}]";
        var contract = _web3.ContractBuilder.Build(abi, target);

        var data = contract.Calldata("increment", new object[]
        {
        });

        var relayResponse = await _web3.Gelato().CallWithSyncFeeErc2771(new CallWithSyncFeeErc2771Request()
        {
            Target = target,
            Data = data,
            FeeToken = feeToken,
            IsRelayContext = true,
        });

        while (true)
        {
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    return
                        $"Task complete. Final status of {status.TaskId}: {status.TaskState}. " +
                        $"Transaction hash: {status.TransactionHash}";
                default:
                    await WaitForSeconds(2);
                    continue;
            }
        }
    }

    /// <summary>
    /// Gelato2771 sponsor call
    /// </summary>
    public async Task<string> SponsorCallErc2771()
    {
        const string target = "0x00172f67db60E5fA346e599cdE675f0ca213b47b";

        const string abi = "[{\"inputs\": []," +
                  "\"name\":\"increment\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";

        var contract = _web3.ContractBuilder.Build(abi, target);

        var data = contract.Calldata("increment");

        var relayResponse = await _web3.Gelato().SponsoredCallErc2771(new SponsoredCallErc2771Request()
        {
            Target = target,
            Data = data,
            User = _web3.Signer.PublicAddress,
        });

        while (true)
        {
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);

            switch (status.TaskState)
            {
                case TaskState.ExecSuccess:
                case TaskState.ExecReverted:
                case TaskState.Cancelled:
                    return
                        $"Task complete. Final status of {status.TaskId}: {status.TaskState}. " +
                        $"Transaction hash: {status.TransactionHash}";
                default:
                    await WaitForSeconds(2);
                    continue;
            }
        }
    }

    public bool GetGelatoDisabled()
    {
        return _web3.Gelato().GetGelatoDisabled();
    }

    async Task WaitForSeconds(int seconds)
    {
        // Task.Delay doesn't work on WebGL
#if UNITY_WEBGL && !UNITY_EDITOR
        var now = Time.time;
        while (Time.time - now < 2)
        {
            await Task.Yield();
        }
#else
        await Task.Delay(seconds * 1000);
#endif
    }

    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<IWeb3InitializedHandler>(this);
        });
    }

    public Task OnWeb3Initialized(Web3 web3)
    {
        _web3 = web3;

        return Task.CompletedTask;
    }
}
