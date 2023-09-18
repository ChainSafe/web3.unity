using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc1155
{
    public class Erc1155BalanceOfBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x2c1867bc3026178a47a677513746dcc6822a137a";
        public string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
        public string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";

        private Erc1155Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc1155Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            var balance = await logic.BalanceOf(contractAddress, account, tokenId);
            SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc1155Sample), nameof(Erc1155Sample.BalanceOf));
        }
    }
}