namespace ChainSafe.Gaming.Evm.Contracts
{
    public interface ICustomContract : IContract
    {
        public string ABI { get; }

        public string ContractAddress { get; set; }

        public Contract OriginalContract { get;  set; }

        public ICustomContract Build(IContractBuilder builder)
        {
            OriginalContract = builder.Build(ABI, ContractAddress);
            return this;
        }
    }
}