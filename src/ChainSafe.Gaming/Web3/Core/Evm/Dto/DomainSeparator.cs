using Nethereum.ABI.EIP712;

namespace ChainSafe.Gaming.Web3.Core.Evm
{
    public static class DomainSeparator
    {
        public static readonly string DomainName = "EIP712Domain";
        public static readonly MemberDescription[] Eip712Domain = new[]
        {
            new MemberDescription { Name = "name", Type = "string" },
            new MemberDescription { Name = "chainId", Type = "uint256" },
            new MemberDescription { Name = "version", Type = "string" },
            new MemberDescription { Name = "verifyingContract", Type = "address" },
        };
    }
}