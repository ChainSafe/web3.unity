using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class GetGasLimitBehaviour : SampleBehaviour
    {
        // todo implement abi storing mechanism
        public string contractAbi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        public string contractAddress = "0x7286Cf0F6E80014ea75Dbc25F545A3be90F4904F";
        public string method = "addTotal";

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var gasLimit = await logic.GetGasLimit(contractAbi, contractAddress, method);

            SampleOutputUtil.PrintResult(gasLimit.ToString(), nameof(UnsortedSample), nameof(UnsortedSample.GetGasLimit));
        }
    }
}