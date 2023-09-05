using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class TransferErc721Behaviour : SampleBehaviour
    {
        public string contractAddress = "0x358AA13c52544ECCEF6B0ADD0f801012ADAD5eE3";
        public string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        public int tokenId = 0;

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var response = await logic.TransferErc721(contractAddress, toAccount, tokenId);

            var output = SampleOutputUtil.BuildOutputValue(response);
            SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.TransferErc721));
        }
    }
}