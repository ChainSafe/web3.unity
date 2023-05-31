using System;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.Hex.HexConvertors.Extensions;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet
{
    public class MetaMaskBrowserSigner : IEvmSigner
    {
        private static readonly TimeSpan MinClipboardCheckPeriod = TimeSpan.FromMilliseconds(10);

        private readonly MetaMaskBrowserSignerConfiguration configuration;
        private readonly IOperatingSystemMediator operatingSystem;
        private readonly IAnalyticsClient analytics;

        private string publicAddress = "undefined";

        public MetaMaskBrowserSigner(
            IEvmProvider provider,
            MetaMaskBrowserSignerConfiguration configuration,
            IOperatingSystemMediator operatingSystem,
            IAnalyticsClient analytics)
        {
            this.analytics = analytics;
            this.operatingSystem = operatingSystem;
            this.configuration = configuration;

            Provider = provider;
            Connected = false;
        }

        public bool Connected { get; private set; }

        public IEvmProvider Provider { get; }

        public async ValueTask Connect()
        {
            this.AssertNotConnected();
            publicAddress = await this.VerifyUserOwnsAccount();
            Connected = true;
        }

        public Task<string> GetAddress()
        {
            if (!Connected)
            {
                throw new Web3Exception(
                    $"Can't retrieve public address. {nameof(MetaMaskBrowserSigner)} is not connected yet.");
            }

            return Task.FromResult(publicAddress);
        }

        public Task<string> SignMessage(byte[] message)
        {
            return SignMessage(message.ToHex());
        }

        public async Task<string> SignMessage(string message)
        {
            var pageUrl = BuildUrl();
            var hash = await OpenPageWaitResponse(pageUrl, ValidateResponse);

            // todo log event on success
            return hash;

            string BuildUrl()
            {
                return $"{configuration.ServiceUrl}" +
                       "?action=sign" +
                       $"&message={Uri.EscapeDataString(message)}";
            }

            // todo validate with regex
            bool ValidateResponse(string response) => response.StartsWith("0x") && response.Length == 132;
        }

        public Task<string> SignTransaction(TransactionRequest transaction)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var pageUrl = BuildUrl();
            var hash = await OpenPageWaitResponse(pageUrl, ValidateResponse);

            // todo log event on success (see example near end of file)
            return await Provider.GetTransaction(hash);

            string BuildUrl()
            {
                return $"{configuration.ServiceUrl}" +
                       "?action=send" +
                       $"&chainId={transaction.ChainId}" +
                       $"&to={transaction.To}" +
                       $"&value={transaction.Value}" +
                       $"&data={transaction.Data}" +
                       $"&gasLimit={transaction.GasLimit}" +
                       $"&gasPrice={transaction.GasPrice}";
            }

            // todo validate with regex
            bool ValidateResponse(string response) => response.StartsWith("0x") && response.Length == 66;
        }

        // todo extract hash from deeplink instead of clipboard?
        private async Task<string> OpenPageWaitResponse(string pageUrl, Func<string, bool> validator)
        {
            operatingSystem.OpenUrl(pageUrl);
            operatingSystem.ClipboardContent = string.Empty;

            var updateDelay = GetUpdatePeriodSafe();
            while (string.IsNullOrEmpty(operatingSystem.ClipboardContent))
            {
                await Task.Delay(updateDelay);
            }

            var response = operatingSystem.ClipboardContent!;
            var validResponse = validator(response);
            if (!validResponse)
            {
                throw new Web3Exception("Incorrect response format extracted from clipboard.");
            }

            return response;

            int GetUpdatePeriodSafe()
            {
                return (int)Math.Max(
                    MinClipboardCheckPeriod.TotalMilliseconds,
                    configuration.ClipboardCheckPeriod.TotalMilliseconds);
            }
        }

        /*
         Logging event on SendTransaction success
        var data = new
        {
            Client = "Desktop/Mobile",
            Version = "v2",
            ProjectID = PlayerPrefs.GetString("ProjectID"),
            Player = Sha3(PlayerPrefs.GetString("Account") + PlayerPrefs.GetString("ProjectID")),
            ChainId = _chainId,
            Address = _to,
            Value = _value,
            GasLimit = _gasLimit,
            GasPrice = _gasPrice,
            Data = _data
        };

        Logging.SendGameData(data);

        public static string Sha3(string _message)
        {
            var signer = new EthereumMessageSigner();
            var hash = new Sha3Keccack().CalculateHash(_message).EnsureHexPrefix();
            // 0x06b3dfaec148fb1bb2b066f10ec285e7c9bf402ab32aa78a5d38e34566810cd2
            return hash;
        }
         */
    }
}