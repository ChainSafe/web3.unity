using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using System.Text;

public class KeyStoreManagerUtils
{
#if UNITY_IOS
    [DllImport("__Internal")]
    extern static int web3auth_keystore_set(string key, string value);

    [DllImport("__Internal")]
    extern static string web3auth_keystore_get(string key);

    [DllImport("__Internal")]
    extern static int web3auth_keystore_delete(string key);
#endif

    public static string SESSION_ID = "sessionId";
    public static string IV_KEY = "ivKey";
    public static string EPHEM_PUBLIC_Key = "ephemPublicKey";
    public static string MAC = "mac";

    public static string getPubKey(string sessionId)
    {
        try
        {
            var domain = SecNamedCurves.GetByName("secp256k1");
            var parameters = new ECDomainParameters(domain.Curve, domain.G, domain.H);

            var key = new ECPrivateKeyParameters(new BigInteger(sessionId, 16), parameters);
            var q = new ECPublicKeyParameters("EC", domain.G.Multiply(key.D), parameters).Q;

            return Hex.ToHexString(domain.Curve.CreatePoint(q.XCoord.ToBigInteger(), q.YCoord.ToBigInteger()).GetEncoded(false));
        } catch (System.Exception ex)
        {
            UnityEngine.Debug.Log(ex);
            return "";
        }
    }

    static KeyStoreManagerUtils()
    {
#if !UNITY_IOS
        SecurePlayerPrefs.Init();
#endif
    }

    public static void savePreferenceData(string key, string value)
    {
#if UNITY_IOS
        web3auth_keystore_set(key, value);
#else
        SecurePlayerPrefs.SetString(key, value);
#endif
    }

    public static string getPreferencesData(string key)
    {
#if UNITY_IOS
        return web3auth_keystore_get(key);
#else
        return SecurePlayerPrefs.GetString(key);
#endif
    }
    public static void deletePreferencesData(string key)
    {
#if UNITY_IOS
        web3auth_keystore_delete(key);
#else
        SecurePlayerPrefs.DeleteKey(key);
#endif
    }

    public static AsymmetricCipherKeyPair generateECKeyPair()
    {
        var secureRandom = new SecureRandom();
        var curve = SecNamedCurves.GetByName("secp256k1");
        var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

        var keyGenParam = new ECKeyGenerationParameters(domainParams, secureRandom);
        var generator = GeneratorUtilities.GetKeyPairGenerator("ECDSA");
        generator.Init(keyGenParam);

        return generator.GenerateKeyPair();
    }   

    public static string generateRandomSessionKey() {
        var keyPair = generateECKeyPair();
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        string privateKeyHex = privateKey.D.ToString(16).PadLeft(64, '0');
        return privateKeyHex;
    }

    public static byte[] generateRandomBytes()
    {
        var secureRandom = new SecureRandom();
        byte[] bytes = new byte[16];
        secureRandom.NextBytes(bytes);
        return bytes;
    }

    public static string getECDSASignature(string privateKey, string data){
        var curve = SecNamedCurves.GetByName("secp256k1");
        var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
        var keyParameters = new ECPrivateKeyParameters(new BigInteger(privateKey, 16), domain);

        var signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));
        signer.Init(true, keyParameters);

        var hashAlgorithm = new KeccakDigest(256);
        byte[] input = System.Text.Encoding.UTF8.GetBytes(data);
        hashAlgorithm.BlockUpdate(input, 0, input.Length);

        byte[] messageHash = new byte[32];
        hashAlgorithm.DoFinal(messageHash, 0);

        var signature = signer.GenerateSignature(messageHash);

        var r = signature[0];
        var s = signature[1];

        var other = curve.Curve.Order.Subtract(s);
        if (s.CompareTo(other) == 1)
            s = other;

        var v = new Asn1EncodableVector();
        v.Add(new DerInteger(r));
        v.Add(new DerInteger(s));

        var derSignature = new DerSequence(v).GetDerEncoded();

        return Hex.ToHexString(derSignature);
    }

    public static string convertByteToHexadecimal(byte[] byteArray)
    {
        string hex = "";
        // Iterating through each byte in the array
        foreach (byte b in byteArray)
        {
            hex += $"{b:X2}";
        }
        return hex.ToLowerInvariant();
    }
}
