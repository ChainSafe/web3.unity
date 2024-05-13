using System.Reflection;
using System.Threading;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Utils;

namespace ChainSafe.Gaming.Evm.Contracts.BuiltIn
{
    public class Erc20Service
    {
        private readonly string abi;
        private readonly IContractBuilder contractBuilder;
        private readonly ISigner signer;

        private Erc20Service()
        {
            abi = AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.erc-20-abi.json");
        }

        public Erc20Service(IContractBuilder contractBuilder)
            : this()
        {
            this.contractBuilder = contractBuilder;
        }

        public Erc20Service(IContractBuilder contractBuilder, ISigner signer)
            : this(contractBuilder)
        {
            this.signer = signer;
        }

        /// <summary>
        /// Builds an ERC20 contract instance with the given address.
        /// </summary>
        /// <param name="address">The address of the ERC20 contract.</param>
        /// <returns>An instance of Erc20Contract.</returns>
        public Erc20Contract BuildContract(string address)
        {
            var originalContract = contractBuilder.Build(abi, address);
            return new Erc20Contract(originalContract, signer);
        }
    }
}