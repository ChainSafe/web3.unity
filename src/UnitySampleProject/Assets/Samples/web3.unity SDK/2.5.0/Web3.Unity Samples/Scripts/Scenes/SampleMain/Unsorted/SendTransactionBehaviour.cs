using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class SendTransactionBehaviour : SampleBehaviour
    {
        public string to = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var transactionHash = await logic.SendTransaction(to);

            SampleOutputUtil.PrintResult(transactionHash, nameof(UnsortedSample), nameof(UnsortedSample.SendTransaction));
        }
    }
}