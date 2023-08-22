using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class GetArrayBehaviour : SampleBehaviour
    {
        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var response = await logic.GetArray();

            var responseString = string.Join(",\n", response.Select((list, i) => $"#{i} {string.Join((string)", ", (IEnumerable<string>)list)}"));
            SampleOutputUtil.PrintResult(responseString, nameof(UnsortedSample), nameof(UnsortedSample.GetArray));
        }
    }
}