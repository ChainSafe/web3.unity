using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer.Crypto;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace Web3Unity.Scripts.Library.Ethers.SigningKey
{
    public class SigningKey
    {
        // private readonly string _curve;
        // private readonly string _privateKey;
        // private readonly string _publicKey;
        private readonly ECKey ecKey;

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

        public SigningKey(byte[] privateKey)
            : this(Bytes.Bytes.Hexlify(privateKey))
        {
        }

        public string PrivateKey => "0x" + ecKey.PrivateKey.D.ToByteArray().ToHex();

        public string CompressedPublicKey => ecKey.GetPubKey(true).ToHex(true);
    }
}