using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using UnityEngine;

public class GelatoCalls : SampleBase<GelatoCalls>
{
    // Fields
    private GelatoSample gelato;

    // Initializes the protocol class
    public void Awake()
    {
        gelato = new GelatoSample(Web3Unity.Web3);
    }

    /// <summary>
    /// Gelato with sync fee
    /// </summary>
    public async Task<string> GelatoCallWithSyncFee()
    {
        var result = await gelato.CallWithSyncFee();
        return SampleOutputUtil.BuildResultMessage(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.CallWithSyncFee));
    }

    /// <summary>
    /// Gelato2771 with sync fee
    /// </summary>
    public async Task<string> GelatoCallWithSyncFeeErc2771()
    {
        var result = await gelato.CallWithSyncFeeErc2771();
        return SampleOutputUtil.BuildResultMessage(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.CallWithSyncFeeErc2771));
    }

    /// <summary>
    /// Gelato sponsor call
    /// </summary>
    public async Task<string> GelatoSponsorCall()
    {
        var result = await gelato.SponsorCall();
        return SampleOutputUtil.BuildResultMessage(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.SponsorCall));
    }

    /// <summary>
    /// Gelato2771 sponsor call
    /// </summary>
    public async Task<string> GelatoSponsorCallErc2771()
    {
        var result = await gelato.SponsorCallErc2771();

        return SampleOutputUtil.BuildResultMessage(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.SponsorCallErc2771));
    }

    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder;
    }
}
