using ChainSafe.Gaming.UnityPackage;
using UnityEngine;

public class GelatoCalls : MonoBehaviour
{
    // Fields
    private GelatoSample gelato;

    // Initializes the protocol class
    public void Awake()
    {
        gelato = new GelatoSample(Web3Accessor.Web3);
    }

    /// <summary>
    /// Gelato with sync fee
    /// </summary>
    public async void GelatoCallWithSyncFee()
    {
        var result = await gelato.CallWithSyncFee();
        SampleOutputUtil.PrintResult(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.CallWithSyncFee));
    }

    /// <summary>
    /// Gelato2771 with sync fee
    /// </summary>
    public async void GelatoCallWithSyncFeeErc2771()
    {
        var result = await gelato.CallWithSyncFeeErc2771();
        SampleOutputUtil.PrintResult(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.CallWithSyncFeeErc2771));
    }

    /// <summary>
    /// Gelato sponsor call
    /// </summary>
    public async void GelatoSponsorCall()
    {
        var result = await gelato.SponsorCall();
        SampleOutputUtil.PrintResult(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.SponsorCall));
    }

    /// <summary>
    /// Gelato2771 sponsor call
    /// </summary>
    public async void GelatoSponsorCallErc2771()
    {
        var result = await gelato.SponsorCallErc2771();

        SampleOutputUtil.PrintResult(
            $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
            $"Transaction hash: {result.Status.TransactionHash}",
            nameof(GelatoSample), nameof(GelatoSample.SponsorCallErc2771));
    }
}
