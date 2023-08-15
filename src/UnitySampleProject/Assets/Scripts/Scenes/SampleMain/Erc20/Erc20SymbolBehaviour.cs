﻿namespace Samples.Behaviours
{
    public class Erc20SymbolBehaviour : ButtonBehaviour
    {
        public string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        
        private Erc20Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc20Sample(Web3Accessor.Web3);
        }

        protected override async void Execute()
        {
            var result = await logic.Symbol(contractAddress);
            SampleOutputUtil.PrintResult(result, nameof(Erc20Sample), nameof(Erc20Sample.Symbol));
        }
    }
}