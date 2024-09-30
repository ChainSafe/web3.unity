using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;

public class GelatoCalls : SampleBase<GelatoCalls>, IWeb3InitializedHandler
{
    // Fields
    private GelatoSample gelato;

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
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<IWeb3InitializedHandler>(this);
        });
    }

    public Task OnWeb3Initialized(Web3 web3)
    {
        gelato = new GelatoSample(web3);

        return Task.CompletedTask;
    }
}
