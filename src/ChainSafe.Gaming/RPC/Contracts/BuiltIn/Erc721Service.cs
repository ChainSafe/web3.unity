using System.Reflection;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Utils;
using ChainSafe.Gaming.MultiCall;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public class Erc721Service
    {
        private readonly string abi;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;
        private readonly IMultiCall multiCall;

        private Erc721Service()
        {
            abi = AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.erc-721-abi.json");
        }

        public Erc721Service(IContractBuilder contractBuilder)
            : this()
        {
            this.contractBuilder = contractBuilder;
        }

        public Erc721Service(IContractBuilder contractBuilder, ISigner signer)
            : this(contractBuilder)
        {
            this.signer = signer;
        }

        public Erc721Service(IContractBuilder contractBuilder, ISigner signer, IMultiCall multiCall)
            : this(contractBuilder, signer)
        {
            this.multiCall = multiCall;
        }

        /// <summary>
        /// Builds an ERC721 contract instance with the given address.
        /// </summary>
        /// <param name="address">The address of the ERC721 contract.</param>
        /// <returns>An instance of Erc721Contract.</returns>
        public Erc721Contract BuildContract(string address)
        {
            var originalContract = contractBuilder.Build(abi, address);
            return new Erc721Contract(originalContract, signer, multiCall);
        }
    }
}