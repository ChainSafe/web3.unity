using System.Threading.Tasks;

namespace Samples.Behaviours.Gelato
{
    public class GelatoSponsorCallErc2771Behaviour : SampleBehaviour
    {
        private GelatoSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new GelatoSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var result = await logic.SponsorCallErc2771();

            SampleOutputUtil.PrintResult(
                $"Task complete. Final status of {result.TaskId}: {result.Status.TaskState}. " +
                $"Transaction hash: {result.Status.TransactionHash}",
                nameof(GelatoSample), nameof(GelatoSample.SponsorCallErc2771));
        }
    }
}