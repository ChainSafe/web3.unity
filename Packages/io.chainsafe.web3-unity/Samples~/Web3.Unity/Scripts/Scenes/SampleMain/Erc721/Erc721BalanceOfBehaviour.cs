using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc721
{
    public class Erc721BalanceOfBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x9123541E259125657F03D7AD2A7D1a8Ec79375BA";
        public string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

        private Erc721Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc721Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            var balance = await logic.BalanceOf(contractAddress, account);
            SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc721Sample), nameof(Erc721Sample.BalanceOf));
        }
    }
}