using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.EVM.InProcessSigner
{
    public class InProcessSigner : ISigner
    {
        private EthECKey privateKey;
        private EthereumMessageSigner messageSigner;

        public InProcessSigner(InProcessSignerConfig config)
        {
            privateKey = config.PrivateKey ??
                throw new Web3Exception($"{nameof(InProcessSignerConfig)}.{nameof(InProcessSignerConfig.PrivateKey)} must be set");
            messageSigner = new();
        }

        public Task<string> GetAddress() => Task.FromResult(privateKey.GetPublicAddress());

        public Task<string> SignMessage(string message) =>
            Task.FromResult(messageSigner.EncodeUTF8AndSign(message, privateKey));

        // TODO: test this with Gelato
        public Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message) =>
            Task.FromResult(
                Eip712TypedDataSigner.Current.SignTypedData(
                    new TypedData<Domain>
                    {
                        // TODO: missing the PrimaryType property, not sure if it's required
                        Domain = domain,
                        Types = types,
                        Message = message,
                    },
                    privateKey));

        public EthECKey GetKey() => privateKey;
    }
}
