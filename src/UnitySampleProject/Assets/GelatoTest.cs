using System.Numerics;
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
using Web3Unity.Scripts.Library.Ethers.Transactions;
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

    public async void TestingGelatoFunction()
    {
        Debug.Log("Testing works");
    }

    private async Task InitWeb3()
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

    public async void CallWithSyncFeeExample()
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
            new BigInteger(5 * 10E12)
        });
        
        var relayResponse = await _web3.Gelato().CallWithSyncFee(new CallWithSyncFeeRequest()
        {
            Data = data,
            FeeToken = feeToken,
            IsRelayContext = true,
            Target = target,
        });

        var complete = false;
        while (!complete)
        {
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);
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

    public async void SponsorCallExample()
    {
        var counterContract = "0x763D37aB388C5cdd2Fb0849d6275802F959fbF30";

        var abi = "[{\"inputs\": []," +
                  "\"name\":\"increment\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";
        var contract = new Contract(abi, counterContract);
        var data = contract.Calldata("increment");

        var relayResponse = await _web3.Gelato().SponsoredCall(new SponsoredCallRequest()
        {
            Target = counterContract,
            Data = data,
        });

        var complete = false;
        while (!complete)
        {
            Thread.Sleep(2000);
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);
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
                    continue;
            }
        }
    }

    public async void CallWithSyncFeeErc2771Example()
    {
        var target = "0x5dD1100f23278e0e27972eacb4F1B81D97D071B7";
        var feeToken = "0xEeeeeEeeeEeEeeEeEeEeeEEEeeeeEeeeeeeeEEeE";
        var abi = "[{\"inputs\": []," +
                  "\"name\":\"increment\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";
        var contract = new Contract(abi, target);
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

        var complete = false;
        while (!complete)
        {
            Thread.Sleep(2000);
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);
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
                    continue;
            }
        }
    }

    public async void sponsorCallErc2771Example()
    {
        var target = "0x00172f67db60E5fA346e599cdE675f0ca213b47b";

        var abi = "[{\"inputs\": []," +
                  "\"name\":\"increment\"," +
                  "\"outputs\":[]," +
                  "\"stateMutability\":\"nonpayable\"," +
                  "\"type\":\"function\"" +
                  "}]";
        var contract = new Contract(abi, target);
        var data = contract.Calldata("increment");

        var relayResponse = await _web3.Gelato().SponsoredCallErc2771(new SponsoredCallErc2771Request()
        {
            Target = target,
            Data = data,
            User = await _web3.Signer.GetAddress(),
        });     

        var complete = false;
        while (!complete)
        {
            Thread.Sleep(2000);
            var status = await _web3.Gelato().GetTaskStatus(relayResponse.TaskId);
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
                    continue;
            }
        }
    }

}
