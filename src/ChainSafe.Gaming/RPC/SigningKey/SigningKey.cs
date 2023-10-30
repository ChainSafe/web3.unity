using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer.Crypto;

namespace ChainSafe.Gaming.Evm.SigningKey
{
    /// <summary>
    /// Creates a new instance of the SigningKey class.
    /// </summary>
    public class SigningKey
    {
        // private readonly string _curve;
        // private readonly string _privateKey;
        // private readonly string _publicKey;
        private readonly ECKey ecKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningKey"/> class with a private key.
        /// </summary>
        /// <param name="privateKey">The private key as a hex string.</param>
        public SigningKey(string privateKey)
        {
            // this.privateKey = privateKey;
            // // TODO: check hex data length == 32
            //
            // var ecCurveByOid = ECNamedCurveTable.GetByOid(X9ObjectIdentifiers.Prime256v1);
            // var parameters = new ECDomainParameters(ecCurveByOid.Curve, ecCurveByOid.G, ecCurveByOid.N, ecCurveByOid.H, ecCurveByOid.GetSeed());
            // var keyPair = new AsymmetricCipherKeyPair((AsymmetricKeyParameter) new ECPublicKeyParameters("EC", q, parameters), (AsymmetricKeyParameter) new ECPrivateKeyParameters("EC", bigInteger, parameters))
            ecKey = new ECKey(privateKey.HexToByteArray(), true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigningKey"/> class with a private key.
        /// </summary>
        /// <param name="privateKey">The private key as a byte array.</param>
        public SigningKey(byte[] privateKey)
            : this(Bytes.Bytes.Hexlify(privateKey))
        {
        }

        /// <summary>
        /// Get the private key.
        /// </summary>
        /// <value>The private key as a hex string.</value>
        public string PrivateKey => "0x" + ecKey.PrivateKey.D.ToByteArray().ToHex();

        /// <summary>
        /// Get the compressed public key.
        /// </summary>
        /// <value>The compressed public key as a hex string.</value>
        public string CompressedPublicKey => ecKey.GetPubKey(true).ToHex(true);
    }
}