using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class SendArrayBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x5244d0453A727EDa96299384370359f4A2B5b20a";
        public string abi = "[{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"_addresses\",\"type\":\"address[]\"}],\"name\":\"setStore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bought\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getStore\",\"outputs\":[{\"internalType\":\"address[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        public string method = "setStore";

        public string[] stringArray =
        {
            "0xFb3aECf08940785D4fB3Ad87cDC6e1Ceb20e9aac",
            "0x92d4040e4f3591e60644aaa483821d1bd87001e3"
        };

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var response = await logic.SendArray(method, abi, contractAddress, stringArray);

            var output = SampleOutputUtil.BuildOutputValue(response);
            SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.SendArray));
        }
    }
}