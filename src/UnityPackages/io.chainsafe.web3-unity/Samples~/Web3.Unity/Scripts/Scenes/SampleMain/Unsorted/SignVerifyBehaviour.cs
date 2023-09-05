using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class SignVerifyBehaviour : SampleBehaviour
    {
        public string message = "A man chooses, a slave obeys.";

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var signatureVerified = await logic.SignVerify(message);

            var output = signatureVerified ? "Verified" : "Failed to verify";
            SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.SignVerify));

        }
    }
}