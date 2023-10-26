using ChainSafe.Gaming.Evm.Signers;
using Nethereum.Signer;

namespace ChainSafe.Gaming.InProcessSigner
{
    /// <summary>
    /// Config for <see cref="InProcessSigner"/>.
    /// </summary>
    public class InProcessSignerConfig
    {
        /// <summary>
        /// Private key of <see cref="ISigner"/>.
        /// </summary>
        public EthECKey? PrivateKey { get; set; }
    }
}
