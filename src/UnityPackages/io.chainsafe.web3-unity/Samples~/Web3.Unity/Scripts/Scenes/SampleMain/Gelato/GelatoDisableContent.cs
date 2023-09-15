using System.Threading.Tasks;

namespace Samples.Behaviours.Gelato
{
    public class GelatoDisableContent : SampleBehaviour
    {
        private GelatoSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new GelatoSample(Web3);
            ExecuteSample();
        }

        protected override Task ExecuteSample()
        {
            if (!logic.GetGelatoDisabled()) return Task.CompletedTask;
            print("Gelato functionality disabled as your chain isn't supported");
            gameObject.SetActive(false);
            return Task.CompletedTask;
        }
    }
}