using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class TransferErc20Behaviour : SampleBehaviour
    {
        public string contractAddress = "0xc778417e063141139fce010982780140aa0cd5ab";
        public string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        public string amount = "1000000000000000"; // todo to double representing one unit of currency

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var response = await logic.TransferErc20(contractAddress, toAccount, amount);

            var output = SampleOutputUtil.BuildOutputValue(response);
            SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.TransferErc20));
        }
    }
}