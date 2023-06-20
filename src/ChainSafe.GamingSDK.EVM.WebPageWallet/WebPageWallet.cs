using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Debug;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Environment;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using Nethereum.Util;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet
{
    public class WebPageWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        private static readonly TimeSpan MinClipboardCheckPeriod = TimeSpan.FromMilliseconds(10);

        private readonly WebPageWalletConfig configuration;
        private readonly IOperatingSystemMediator operatingSystem;
        private readonly IRpcProvider provider;

        private string? address;

        public WebPageWallet(
            IRpcProvider provider,
            WebPageWalletConfig configuration,
            IOperatingSystemMediator operatingSystem)
        {
            this.provider = provider;
            this.operatingSystem = operatingSystem;
            this.configuration = configuration;
        }

        public delegate string ConnectMessageBuildDelegate(DateTime expirationTime);

        public async ValueTask WillStartAsync()
        {
            address = await GetAccountVerifyUserOwns();
        }

        public ValueTask WillStopAsync() => new(Task.CompletedTask);

        public Task<string> GetAddress()
        {
            address.AssertNotNull(nameof(address));
            return Task.FromResult(address!);
        }

        public async Task<string> SignMessage(string message)
        {
            var pageUrl = BuildUrl();
            var hash = await OpenPageWaitResponse(pageUrl, ValidateResponse);

            // todo: log event on success
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

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var pageUrl = BuildUrl();
            var hash = await OpenPageWaitResponse(pageUrl, ValidateResponse);

            // todo: log event on success (see example near end of file)
            return await provider.GetTransaction(hash);

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

        public async Task<string> SignTypedData(Domain domain, Dictionary<string, MemberDescription[]> types, MemberValue[] message)
        {
            var pageUrl = BuildUrl();
            return await OpenPageWaitResponse(pageUrl, ValidateResponse);

            string BuildUrl()
            {
                return $"{configuration.ServiceUrl}" +
                       "?action=sign-typed-data" +
                       "&domain=" + JsonConvert.SerializeObject(domain) +
                       "&types=" + JsonConvert.SerializeObject(types) +
                       "&message=" + Uri.EscapeDataString(JsonConvert.SerializeObject(message));
            }

            // todo validate with regex
            bool ValidateResponse(string response) => response.StartsWith("0x") && response.Length == 132;
        }

        // todo: extract hash from deeplink instead of clipboard
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

        private async Task<string> GetAccountVerifyUserOwns()
        {
            // sign current time
            var expirationTime = DateTime.Now + configuration.ConnectRequestExpiresAfter;
            var message = configuration.ConnectMessageBuilder(expirationTime);
            var signature = await SignMessage(message);
            var publicAddress = ExtractPublicAddress(signature, message);

            if (publicAddress.Length != 42)
            {
                throw new Web3Exception(
                    $"Public address recovered from signature has length {publicAddress.Length}. Expected length: 42.");
            }

            if (DateTime.Now > expirationTime)
            {
                throw new Web3Exception("Signature has already expired. Try again.");
            }

            return publicAddress;

            string ExtractPublicAddress(string signature, string originalMessage)
            {
                var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
                var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
                var ecdsaSignature = MessageSigner.ExtractEcdsaSignature(signature);
                var key = EthECKey.RecoverFromSignature(ecdsaSignature, msgHash);
                return key.GetPublicAddress();
            }
        }

        /*
         Storing this here just to know, how events for analytics were constructed

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