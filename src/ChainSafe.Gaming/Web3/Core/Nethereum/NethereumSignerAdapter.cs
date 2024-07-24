using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Signers;
using Nethereum.Model;
using Nethereum.Signer;
using Nethereum.Signer.Crypto;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public class NethereumSignerAdapter : EthExternalSignerBase
    {
        private readonly ISigner signer;

        public NethereumSignerAdapter(ISigner signer)
        {
            this.signer = signer;
        }

        public override bool CalculatesV { get; protected set; } = true;

        public override bool Supported1559 { get; } = true;

        public override ExternalSignerTransactionFormat ExternalSignerTransactionFormat { get; protected set; } =
            ExternalSignerTransactionFormat.Hash;

        public override Task<string> GetAddressAsync()
        {
            return Task.FromResult(signer.PublicAddress);
        }

        protected override Task<byte[]> GetPublicKeyAsync()
        {
            throw new Web3Exception("Not implemented interface to retrieve the public key.");
        }

        protected override async Task<ECDSASignature> SignExternallyAsync(byte[] bytes)
        {
            return await SignExternallyAsyncInternal(bytes).ConfigureAwait(false);
        }

        private async Task<ECDSASignature> SignExternallyAsyncInternal(byte[] bytes)
        {
            var stringMessage = Encoding.UTF8.GetString(bytes);
            var stringSignature = await signer.SignMessage(stringMessage).ConfigureAwait(false);
            var bytesSignature = Encoding.UTF8.GetBytes(stringSignature);
            return ECDSASignatureFactory.ExtractECDSASignature(bytesSignature);
        }

        public override Task SignAsync(LegacyTransactionChainId transaction) => SignHashTransactionAsync(transaction);

        public override Task SignAsync(Transaction1559 transaction) => SignHashTransactionAsync(transaction);

        public override Task SignAsync(LegacyTransaction transaction) => SignHashTransactionAsync(transaction);
    }
}