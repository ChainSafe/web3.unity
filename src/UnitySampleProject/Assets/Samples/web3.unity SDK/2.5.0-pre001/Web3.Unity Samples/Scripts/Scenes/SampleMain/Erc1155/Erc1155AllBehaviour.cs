using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc1155
{
    public class Erc1155AllBehaviour : SampleBehaviour
    {
        public string chain = "ethereum";
        public string network = "goerli"; // mainnet goerli
        public string account = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";
        [Header("Optional")]
        public string contract = "";
        public int take = 1000;
        public int skip = 0;

        private Erc1155Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc1155Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            var allNfts = await logic.All(chain, network, account, contract, take, skip);
            var output = string.Join(",\n", allNfts.Select(nft => $"{nft.TokenId} - {nft.Uri}"));
            SampleOutputUtil.PrintResult(output, nameof(Erc1155Sample), nameof(Erc1155Sample.All));
        }
    }
}