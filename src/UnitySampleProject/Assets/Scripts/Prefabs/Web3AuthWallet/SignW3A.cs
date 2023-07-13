using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using UnityEngine.UI;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Web3AuthWallet;

public class SignW3A : MonoBehaviour, ISigner
{
    public Text responseText;
    public string message = "This is a test message to sign";
    private EthereumMessageSigner _signer;

    private void Awake()
    {
        _signer = new EthereumMessageSigner();
    }

    public void OnEnable()
    {
        // resets response text
        responseText.text = string.Empty;
    }

    public void OnSignMessage()
    {
        var signed = SignMessage(message).Result;
        responseText.text = signed;
    }

    public Task<string> GetAddress()
    {
        throw new System.NotImplementedException();
    }

    public Task<string> SignMessage(string message)
    {
        var signedMessage = _signer.EncodeUTF8AndSign(message, new EthECKey(W3AWalletUtils.PrivateKey));
        return Task.FromResult(signedMessage);
    }

    public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
    {
        throw new System.NotImplementedException();
    }
}