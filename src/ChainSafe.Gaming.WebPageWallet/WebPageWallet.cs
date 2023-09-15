using System;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Debug;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.ABI.EIP712;
using Nethereum.Signer;
using Nethereum.Util;
using Newtonsoft.Json;
using AddressExtensions = ChainSafe.Gaming.Web3.Core.Debug.AddressExtensions;

namespace ChainSafe.Gaming.Wallets
{
    public class WebPageWallet : ISigner, ITransactionExecutor, ILifecycleParticipant
    {
        public delegate string ConnectMessageBuildDelegate(DateTime expirationTime);

#pragma warning disable SA1201
        private static readonly TimeSpan MinClipboardCheckPeriod = TimeSpan.FromMilliseconds(10);
#pragma warning restore SA1201
        private readonly IChainConfig chainConfig;

        private readonly WebPageWalletConfig configuration;
        private readonly IOperatingSystemMediator operatingSystem;
        private readonly IRpcProvider provider;

        private string? address;

        public WebPageWallet(
            IRpcProvider provider,
            WebPageWalletConfig configuration,
            IOperatingSystemMediator operatingSystem,
            IChainConfig chainConfig)
        {
            this.provider = provider;
            this.operatingSystem = operatingSystem;
            this.chainConfig = chainConfig;
            this.configuration = configuration;
        }

        public async ValueTask WillStartAsync()
        {
            configuration.SavedUserAddress?.AssertIsPublicAddress(nameof(configuration.SavedUserAddress));
            address = configuration.SavedUserAddress ?? await GetAccountVerifyUserOwns();
        }

        public ValueTask WillStopAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public Task<string> GetAddress()
        {
            address.AssertNotNull(nameof(address));
            return Task.FromResult(address!);
        }

        public async Task<string> SignMessage(string message)
        {
            var pageUrl = BuildUrl();
            var hash = await OpenPageWaitResponse(pageUrl, ValidateResponse);

            // TODO: log event on success
            return hash;

            string BuildUrl()
            {
                return $"{configuration.ServiceUrl}" +
                       "?action=sign" +
                       $"&message={Uri.EscapeDataString(message)}";
            }

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 132;
            }
        }

        public async Task<string> SignTypedData<TStructType>(SerializableDomain domain, TStructType message)
        {
            var pageUrl = BuildUrl();
            return await OpenPageWaitResponse(pageUrl, ValidateResponse);

            string BuildUrl()
            {
                return $"{configuration.ServiceUrl}" +
                       "?action=sign-typed-data" +
                       "&domain=" + Uri.EscapeDataString(JsonConvert.SerializeObject(domain)) +
                       "&types=" + Uri.EscapeDataString(JsonConvert.SerializeObject(
                           MemberDescriptionFactory.GetTypesMemberDescription(typeof(TStructType)))) +
                       "&message=" + Uri.EscapeDataString(JsonConvert.SerializeObject(message));
            }

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 132;
            }
        }

        public async Task<TransactionResponse> SendTransaction(TransactionRequest transaction)
        {
            var pageUrl = BuildUrl();
            var hash = await OpenPageWaitResponse(pageUrl, ValidateResponse);

            // TODO: log event on success (see example near end of file)
            return await provider.GetTransaction(hash);

            string BuildUrl()
            {
                var sb = new StringBuilder()
                    .Append(configuration.ServiceUrl)
                    .Append("?action=send");

                if (transaction.ChainId != null)
                {
                    sb.Append("&chainId=").Append(transaction.ChainId);
                }
                else
                {
                    sb.Append("&chainId=").Append(chainConfig.ChainId);
                }

                if (transaction.Value != null)
                {
                    sb.Append("&value=").Append(transaction.Value);
                }
                else
                {
                    sb.Append("&value=").Append(0);
                }

                AppendStringIfNotNullOrEmtpy("to", transaction.To);
                AppendStringIfNotNullOrEmtpy("data", transaction.Data);
                AppendIfNotNull("gasLimit", transaction.GasLimit);
                AppendIfNotNull("gasPrice", transaction.GasPrice);

                return sb.ToString();

                void AppendIfNotNull(string name, object value)
                {
                    if (value != null)
                    {
                        sb!.Append('&').Append(name).Append('=').Append(value);
                    }
                }

                void AppendStringIfNotNullOrEmtpy(string name, string value)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        sb!.Append('&').Append(name).Append('=').Append(value);
                    }
                }
            }

            // TODO: validate with regex
            bool ValidateResponse(string response)
            {
                return response.StartsWith("0x") && response.Length == 66;
            }
        }

        // TODO: extract hash from deeplink instead of clipboard
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

            if (!Web3.Core.Debug.AddressExtensions.IsPublicAddress(publicAddress))
            {
                throw new Web3Exception(
                    $"Public address recovered from signature is not valid. Public address: {publicAddress}");
            }

            if (DateTime.Now > expirationTime)
            {
                throw new Web3Exception("Signature has already expired. Try again.");
            }

            return publicAddress;

            string ExtractPublicAddress(string sig, string originalMessage)
            {
                try
                {
                    var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
                    var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
                    var ecdsaSignature = MessageSigner.ExtractEcdsaSignature(sig);
                    var key = EthECKey.RecoverFromSignature(ecdsaSignature, msgHash);
                    return key.GetPublicAddress();
                }
                catch
                {
                    throw new Web3Exception("Invalid signature");
                }
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