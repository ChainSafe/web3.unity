using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using Prefabs.Web3AuthWallet.Utils;
using UnityEngine;
using UnityEngine.UI;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class VerifyW3A : MonoBehaviour, ISigner
{
    public Text responseText;
    public string message = "hello";
    private EthereumMessageSigner _signer;
    private SignatureVerifier _verifier;

    private void Awake()
    {
        _verifier = new SignatureVerifier();
        _signer = new EthereumMessageSigner();
    }

    public void OnEnable()
    {
        // resets response text
        responseText.text = string.Empty;
    }

    public void UserSign()
    {
        var signatureResponse = SignMessage(message).Result;
        responseText.text = _verifier.VerifySignature(signatureResponse, message);
    }

    public Task<string> GetAddress()
    {
        throw new System.NotImplementedException();
    }

    public Task<string> SignMessage(string message)
    {
        var signatureResponse = _signer.EncodeUTF8AndSign(message, new EthECKey(W3AWalletUtils.PrivateKey));
        return Task.FromResult(signatureResponse);
    }

    public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
    {
        throw new System.NotImplementedException();
    }
}