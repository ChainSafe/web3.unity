using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class RegisteredContractBehaviour : SampleBehaviour
    {
        public string contractName = "shiba";
        public string method = "balanceOf";
        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var balance = await logic.UseRegisteredContract(contractName, method);

            SampleOutputUtil.PrintResult(balance.ToString(), nameof(UnsortedSample), nameof(UnsortedSample.UseRegisteredContract));
        }
    }
}