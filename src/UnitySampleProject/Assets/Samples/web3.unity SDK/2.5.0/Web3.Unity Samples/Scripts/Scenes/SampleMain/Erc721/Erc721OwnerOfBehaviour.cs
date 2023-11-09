﻿using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc721
{
    public class Erc721OwnerOfBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
        public string tokenId = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

        private Erc721Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc721Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            // check if tokenId is int or hex string
            var owner = tokenId.StartsWith("0x") ? 
                await logic.OwnerOf(contractAddress, tokenId) : await logic.OwnerOf(contractAddress, BigInteger.Parse(tokenId));
            SampleOutputUtil.PrintResult(owner, nameof(Erc721Sample), nameof(Erc721Sample.OwnerOf));
        }
    }
}