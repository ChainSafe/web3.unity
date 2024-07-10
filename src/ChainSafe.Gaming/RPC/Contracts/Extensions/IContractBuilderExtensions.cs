namespace ChainSafe.Gaming.Evm.Contracts.Extensions
{
    public static class IContractBuilderExtensions
    {
        public static T BuildCustomContract<T>(this IContractBuilder builder, string address)
            where T : ICustomContract, new()
        {
            var contract = new T
            {
                ContractAddress = address,
            };

            return (T)contract.Build(builder);
        }
    }
}