using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc1155
{
    public class Erc1155ImportNftTextureBehaviour : SampleBehaviour
    {
        private Erc1155Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc1155Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            await logic.ImportNftTexture();
        }
    }
}