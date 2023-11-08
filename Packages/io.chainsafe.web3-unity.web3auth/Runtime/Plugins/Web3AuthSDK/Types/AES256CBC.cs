using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.IO;
using System;
using System.Text;

public class AES256CBC
{
    private static string TRANSFORMATION = "AES/CBC/PKCS7PADDING";
    private byte[] AES_ENCRYPTION_KEY;
    private byte[] ENCRYPTION_IV;

    private byte[] MAC_KEY;
    private byte[] ENCRYPTION_EPHEM_KEY;

    public AES256CBC(string privateKeyHex, string ephemPublicKeyHex, string encryptionIvHex)
    {
        using (SHA512 shaM = new SHA512Managed())
        {
            var e = ecdh(privateKeyHex, ephemPublicKeyHex);
            var b = toByteArray(e);

            byte[] hash = shaM.ComputeHash(b);
            byte[] encKeyBytes = new byte[32];

            System.Array.Copy(hash, encKeyBytes, 32);

            AES_ENCRYPTION_KEY = encKeyBytes;

            MAC_KEY = new byte[hash.Length - 32];
            System.Array.Copy(hash, 32, MAC_KEY, 0, MAC_KEY.Length);

            ENCRYPTION_IV = toByteArray(encryptionIvHex);
            ENCRYPTION_EPHEM_KEY = toByteArray(ephemPublicKeyHex);
        }
    }

    public byte[] encrypt(byte[] src)
    {
        var key = ParameterUtilities.CreateKeyParameter("AES", AES_ENCRYPTION_KEY);
        var parametersWithIv = new ParametersWithIV(key, ENCRYPTION_IV);

        var cipher = CipherUtilities.GetCipher(TRANSFORMATION);
        cipher.Init(true, parametersWithIv);

        return cipher.DoFinal(src);
    }

    public byte[] decrypt(byte[] src, string mac)
    {
        if (!hmacSha256Verify(MAC_KEY, getCombinedData(src), mac))
        {
            throw new SystemException("Bad MAC error during decrypt");
        }
        var key = ParameterUtilities.CreateKeyParameter("AES", AES_ENCRYPTION_KEY);
        var parametersWithIv = new ParametersWithIV(key, ENCRYPTION_IV);

        var cipher = CipherUtilities.GetCipher(TRANSFORMATION);
        cipher.Init(false, parametersWithIv);

        return cipher.DoFinal(src);
    }


    public BigInteger ecdh(string privateKeyHex, string ephemPublicKeyHex)
    {
        var domain = SecNamedCurves.GetByName("secp256k1");
        var parameters = new ECDomainParameters(domain.Curve, domain.G, domain.H);

        ECPrivateKeyParameters privKey = new ECPrivateKeyParameters(new BigInteger(privateKeyHex, 16), parameters);

        ECDHBasicAgreement basicAgreement = new ECDHBasicAgreement();
        basicAgreement.Init(privKey);

        var pt = domain.Curve.DecodePoint(new BigInteger(ephemPublicKeyHex, 16).ToByteArray());

        return basicAgreement.CalculateAgreement(new ECPublicKeyParameters(pt, parameters));
    }

    public static byte[] toByteArray(string s)
    {
        int len = s.Length;
        byte[] data = new byte[len / 2];
        for (int i = 0; i < len; i += 2)
        {
            data[i / 2] = (byte)(
                (System.Convert.ToInt32(s[i].ToString(), 16) << 4) +
                System.Convert.ToInt32(s[i + 1].ToString(), 16)
            );
        }
        return data;
    }

    public static byte[] toByteArray(BigInteger bi)
    {
        byte[] b = bi.ToByteArray();
        if (b.Length > 1 && b[0] == 0)
        {
            int n = b.Length - 1;
            byte[] newArray = new byte[n];
            System.Array.Copy(b, 1, newArray, 0, n);
            b = newArray;
        }
        return b;
    }

    public byte[] getCombinedData(byte[] cipherTextBytes)
    {
        using (MemoryStream outputStream = new MemoryStream())
        {
            outputStream.Write(ENCRYPTION_IV, 0, ENCRYPTION_IV.Length);
            outputStream.Write(ENCRYPTION_EPHEM_KEY, 0, ENCRYPTION_EPHEM_KEY.Length);
            outputStream.Write(cipherTextBytes, 0, cipherTextBytes.Length);
            return outputStream.ToArray();
        }
    }

    public byte[] getMac(byte[] cipherTextBytes)
    {
        return hmacSha256Sign(MAC_KEY, getCombinedData(cipherTextBytes));
    }

    public byte[] hmacSha256Sign(byte[] key, byte[] data)
    {
        using (HMACSHA256 hmac = new HMACSHA256(key))
        {
            return hmac.ComputeHash(data);
        }
    }

    public bool hmacSha256Verify(byte[] key, byte[] data, string sig)
    {
        byte[] expectedSig = hmacSha256Sign(key, data);
        string expectedSigHex = BitConverter.ToString(expectedSig).Replace("-", "").ToLower();
        return expectedSigHex.Equals(sig);
    }
}
