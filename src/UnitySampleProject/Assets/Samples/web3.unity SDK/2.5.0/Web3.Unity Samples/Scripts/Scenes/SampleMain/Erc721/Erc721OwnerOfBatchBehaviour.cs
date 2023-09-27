using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc721
{
    public class Erc721OwnerOfBatchBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x47381c5c948254e6e0E324F1AA54b7B24104D92D";
        public List<string> tokenIds = new() { "33", "29" };

        [Header("Optional")]
        // optional: multicall contract https://github.com/makerdao/multicall
        public string multicall = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e";

        private Erc721Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc721Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            var owners = await logic.OwnerOfBatch(contractAddress, tokenIds.ToArray(), multicall);
            var ownersString = $"{owners.Count} owner(s):\n" + string.Join(",\n", owners);
            SampleOutputUtil.PrintResult(ownersString, nameof(Erc721Sample), nameof(Erc721Sample.OwnerOfBatch));
        }
    }
}