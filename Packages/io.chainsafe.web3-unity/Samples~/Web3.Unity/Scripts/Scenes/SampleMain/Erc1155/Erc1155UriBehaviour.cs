using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc1155
{
    public class Erc1155UriBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x2c1867BC3026178A47a677513746DCc6822A137A";
        public string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";

        private Erc1155Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc1155Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            var uri = await logic.Uri(contractAddress, tokenId);
            SampleOutputUtil.PrintResult(uri, nameof(Erc1155Sample), nameof(Erc1155Sample.Uri));
        }
    }
}