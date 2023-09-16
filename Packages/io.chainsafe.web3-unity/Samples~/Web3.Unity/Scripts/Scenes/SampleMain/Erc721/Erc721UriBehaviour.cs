using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc721
{
    public class Erc721UriBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
        public string tokenId = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

        private Erc721Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc721Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            var uri = await logic.Uri(contractAddress, tokenId);
            SampleOutputUtil.PrintResult(uri, nameof(Erc721Sample), nameof(Erc721Sample.Uri));
        }
    }
}