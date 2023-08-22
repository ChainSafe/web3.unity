using System.Threading.Tasks;
using Samples.Behaviours;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Prefabs;

namespace Scenes.SampleNftTexture
{
    public class Erc1155ImportNftTextureBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x162BA1d478948e0ab2d4B21dca2471982C1Fb797"; // gitleaks:allow
        public string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5"; // gitleaks:allow

        public RawImage textureContainer;

        private Erc1155Sample logic;
        private Texture nullTexture;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc1155Sample(Web3);
            nullTexture = textureContainer.texture;
        }

        protected override async Task ExecuteSample()
        {
            textureContainer.texture = nullTexture;
            textureContainer.texture = await logic.ImportNftTexture(contractAddress, tokenId);
            SampleOutputUtil.PrintResult("Texture loaded", nameof(Erc1155Sample), nameof(Erc1155Sample.ImportNftTexture));
        }
    }
}