using System.Reflection;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Utils;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public class Erc1155Service
    {
        private readonly string abi;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;

        private Erc1155Service()
        {
            abi = AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.erc-1155-abi.json");
        }

        public Erc1155Service(IContractBuilder contractBuilder)
            : this()
        {
            this.contractBuilder = contractBuilder;
        }

        public Erc1155Service(IContractBuilder contractBuilder, ISigner signer)
            : this(contractBuilder)
        {
            this.signer = signer;
        }

        /// <summary>
        /// Builds an ERC1155 contract instance.
        /// </summary>
        /// <param name="address">The address of the ERC1155 contract.</param>
        /// <returns>An instance of Erc1155Contract.</returns>
        public Erc1155Contract BuildContract(string address)
        {
            var originalContract = contractBuilder.Build(abi, address);
            return new Erc1155Contract(originalContract, signer);
        }
    }
}