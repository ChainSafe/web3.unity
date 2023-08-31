using System.Threading.Tasks;
using Samples.Behaviours;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Prefabs;

namespace Scenes.SampleNftTexture
{
    public class Erc1155ImportNftTextureBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x0288B4F1389ED7b3d3f9C7B73d4408235c0CBbc6";
        public string tokenId = "0";

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