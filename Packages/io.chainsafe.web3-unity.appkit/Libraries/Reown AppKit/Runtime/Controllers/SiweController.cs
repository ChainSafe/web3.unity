using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Reown.AppKit.Unity
{
    public class SiweController
    {
        public bool IsEnabled
        {
            get => Config is { Enabled: true };
        }

        public SiweConfig Config
        {
            get => AppKit.Config.siweConfig;
        }

        public const string SessionPlayerPrefsKey = "RE_SIWE_SESSION";

        public SiweController()
        {
            if (AppKit.Config.siweConfig == null)
            {
                return;
            }

            if (AppKit.Config.siweConfig.GetMessageParams == null)
            {
                throw new InvalidOperationException("GetMessageParams function is required in SiweConfig.");
            }

            AppKit.AccountDisconnected += AccountDisconnectedHandler;
            AppKit.ChainChanged += ChainChangedHandler;
            AppKit.AccountChanged += AccountChangedHandler;
            AppKit.ConnectorController.SignatureRequested += SignatureRequestedHandler;
        }

        public async ValueTask<string> GetNonceAsync()
        {
            if (Config.GetNonce != null)
            {
                return await Config.GetNonce();
            }

            return SiweUtils.GenerateNonce();
        }

        public async ValueTask<SiweMessage> CreateMessageAsync(string ethAddress, string ethChainId)
        {
            var nonce = await GetNonceAsync();
            var messageParams = AppKit.Config.siweConfig.GetMessageParams();

            var createMessageArgs = new SiweCreateMessageArgs(messageParams)
            {
                Nonce = nonce,
                Address = ethAddress,
                ChainId = ethChainId
            };

            var message = Config.CreateMessage != null
                ? Config.CreateMessage(createMessageArgs)
                : SiweUtils.FormatMessage(createMessageArgs);

            return new SiweMessage
            {
                Message = message,
                CreateMessageArgs = createMessageArgs
            };
        }

        public async ValueTask<bool> VerifyMessageAsync(SiweVerifyMessageArgs args)
        {
            if (Config.VerifyMessage != null)
            {
                return await Config.VerifyMessage(args);
            }

            return await args.Cacao.VerifySignature(AppKit.Config.projectId);
        }

        public async ValueTask<SiweSession> GetSessionAsync(GetSiweSessionArgs args)
        {
            Assert.IsTrue(Array.TrueForAll(args.ChainIds, chainId => !Core.Utils.IsValidChainId(chainId)), "Chain IDs must be Ethereum chain IDs.");
            Assert.IsFalse(Core.Utils.IsValidAccountId(args.Address), "Address must be an Ethereum address.");

            SiweSession session = null;
            if (Config.GetSession != null)
            {
                session = await Config.GetSession(args);
            }
            else
            {
                session = new SiweSession(args);
            }

            var json = JsonConvert.SerializeObject(session);
            PlayerPrefs.SetString(SessionPlayerPrefsKey, json);

            Config.OnSignInSuccess(session);

            return session;
        }

        public async ValueTask SignOutAsync()
        {
            PlayerPrefs.DeleteKey(SessionPlayerPrefsKey);

            if (Config.SignOut != null)
            {
                await Config.SignOut();
            }

            Config.OnSignOutSuccess();
        }

        public static bool TryLoadSiweSessionFromStorage(out SiweSession session)
        {
            var siweSessionJson = PlayerPrefs.GetString(SessionPlayerPrefsKey);
            if (string.IsNullOrWhiteSpace(siweSessionJson))
            {
                session = null;
                return false;
            }

            session = JsonConvert.DeserializeObject<SiweSession>(siweSessionJson);
            return true;
        }

        private async void AccountDisconnectedHandler(object sender, Connector.AccountDisconnectedEventArgs e)
        {
            if (IsEnabled && Config.SignOutOnWalletDisconnect)
            {
                await SignOutAsync();
            }
        }

        private async void ChainChangedHandler(object sender, NetworkController.ChainChangedEventArgs e)
        {
            if (!IsEnabled || !Config.SignOutOnChainChange)
                return;

            if (!TryLoadSiweSessionFromStorage(out var siweSession))
                return;

            if (siweSession.EthChainIds.Contains(e.NewChain.ChainReference))
                return;

            await SignOutAsync();
            AppKit.ConnectorController.OnSignatureRequested();
        }

        private async void AccountChangedHandler(object sender, Connector.AccountChangedEventArgs e)
        {
            if (!IsEnabled || !Config.SignOutOnAccountChange)
                return;

            if (!TryLoadSiweSessionFromStorage(out var siweSession))
                return;

            if (!string.Equals(siweSession.EthAddress, e.Account.Address, StringComparison.InvariantCultureIgnoreCase))
            {
                await SignOutAsync();
                AppKit.ConnectorController.OnSignatureRequested();
            }
        }

        private void SignatureRequestedHandler(object sender, SignatureRequest e)
        {
            if (Config.Enabled && Config.OpenSiweViewOnSignatureRequest)
            {
                AppKit.OpenModal(ViewType.Siwe);
            }
        }
    }
}