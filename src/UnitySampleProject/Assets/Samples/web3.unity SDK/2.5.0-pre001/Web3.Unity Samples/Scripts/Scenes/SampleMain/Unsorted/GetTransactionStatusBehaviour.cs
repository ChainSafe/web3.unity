using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class GetTransactionStatusBehaviour : SampleBehaviour
    {
        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var receipt = await logic.GetTransactionStatus();

            var output = $"Confirmations: {receipt.Confirmations}," +
                         $" Block Number: {receipt.BlockNumber}," +
                         $" Status {receipt.Status}";

            SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.GetTransactionStatus));
        }
    }
}