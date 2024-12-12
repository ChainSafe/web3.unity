using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reown.Core.Common.Model.Errors;
using Reown.Sign.Models.Cacao;
using Reown.Sign.Unity;
using UnityEngine;

namespace Reown.AppKit.Unity
{
    public abstract class Connector
    {
        public string ImageId { get; protected set; }

        public ConnectorType Type { get; protected set; }

        public bool IsInitialized { get; protected set; }

        public IEnumerable<Chain> DappSupportedChains { get; protected set; }

        public virtual bool IsAccountConnected { get; protected set; }

        public event EventHandler<SignatureRequest> SignatureRequested;
        public event EventHandler<AccountConnectedEventArgs> AccountConnected;
        public event EventHandler<AccountDisconnectedEventArgs> AccountDisconnected;
        public event EventHandler<AccountChangedEventArgs> AccountChanged;
        public event EventHandler<ChainChangedEventArgs> ChainChanged;

        private readonly HashSet<ConnectionProposal> _connectionProposals = new();

        protected Connector()
        {
        }

        public async Task InitializeAsync(AppKitConfig config, SignClientUnity signClient)
        {
            if (IsInitialized)
                throw new Exception("Already initialized"); // TODO: use custom ex type

            await InitializeAsyncCore(config, signClient);
            IsInitialized = true;
        }

        public async Task<bool> TryResumeSessionAsync()
        {
            if (!IsInitialized)
                throw new Exception("Connector not initialized"); // TODO: use custom ex type

            if (IsAccountConnected)
                throw new Exception("Account already connected"); // TODO: use custom ex type

            var isResumed = await TryResumeSessionAsyncCore();

            if (!isResumed)
                return false;

            IsAccountConnected = true;
            OnAccountConnected(new AccountConnectedEventArgs(GetAccountAsync, GetAccountsAsync));

            return true;
        }

        public ConnectionProposal Connect()
        {
            if (!IsInitialized)
                throw new Exception("Connector not initialized"); // TODO: use custom ex type

            var connection = ConnectCore();

            connection.Connected += ConnectionConnectedHandler;

            _connectionProposals.Add(connection);

            return connection;
        }

        public async Task DisconnectAsync()
        {
            await DisconnectAsyncCore();
        }

        public async Task ChangeActiveChainAsync(Chain chain)
        {
            if (!IsAccountConnected)
                throw new Exception("No account connected"); // TODO: use custom ex type

            await ChangeActiveChainAsyncCore(chain);
        }

        public Task<Account> GetAccountAsync()
        {
            if (!IsAccountConnected)
                throw new Exception("No account connected"); // TODO: use custom ex type

            return GetAccountAsyncCore();
        }

        public Task<Account[]> GetAccountsAsync()
        {
            if (!IsAccountConnected)
                throw new Exception("No account connected"); // TODO: use custom ex type

            return GetAccountsAsyncCore();
        }

        protected virtual void ConnectionConnectedHandler(ConnectionProposal connectionProposal)
        {
            OnAccountConnected(new AccountConnectedEventArgs(GetAccountAsync, GetAccountsAsync));
            if (connectionProposal.IsSignarureRequested)
            {
                OnSignatureRequested();
            }
        }

        protected virtual async Task ApproveSignatureRequestAsync()
        {
            // Wait 1 second before sending personal_sign request
            // to make sure the connection is fully established.
            await Task.Delay(TimeSpan.FromSeconds(1));

            try
            {
                var account = await GetAccountAsyncCore();
                var ethAddress = account.Address;
                var ethChainId = Core.Utils.ExtractChainReference(account.ChainId);

                var siweMessage = await AppKit.SiweController.CreateMessageAsync(ethAddress, ethChainId);

                var signature = await AppKit.Evm.SignMessageAsync(siweMessage.Message);
                var cacaoPayload = SiweUtils.CreateCacaoPayload(siweMessage.CreateMessageArgs);
                var cacaoSignature = new CacaoSignature(CacaoSignatureType.Eip191, signature);
                var cacao = new CacaoObject(CacaoHeader.Caip112, cacaoPayload, cacaoSignature);

                var isSignatureValid = await AppKit.SiweController.VerifyMessageAsync(new SiweVerifyMessageArgs
                {
                    Message = siweMessage.Message,
                    Signature = signature,
                    Cacao = cacao
                });

                if (isSignatureValid)
                {
                    _ = await AppKit.SiweController.GetSessionAsync(new GetSiweSessionArgs
                    {
                        Address = ethAddress,
                        ChainIds = new[]
                        {
                            ethChainId
                        }
                    });

                    OnAccountConnected(new AccountConnectedEventArgs(GetAccountAsync, GetAccountsAsync));
                }
                else
                {
                    await DisconnectAsync();
                }
            }
            catch (Exception e)
            {
                if (e is not ReownNetworkException)
                    Debug.LogException(e);

                await DisconnectAsync();
            }
        }

        protected virtual async Task RejectSignatureAsync()
        {
            await DisconnectAsync();
        }

        internal virtual void OnSignatureRequested()
        {
            SignatureRequested?.Invoke(this, new SignatureRequest
            {
                Connector = this,
                ApproveAsync = ApproveSignatureRequestAsync,
                RejectAsync = RejectSignatureAsync
            });
        }

        protected virtual void OnAccountConnected(AccountConnectedEventArgs e)
        {
            foreach (var c in _connectionProposals)
                c.Dispose();

            _connectionProposals.Clear();
            IsAccountConnected = true;
            AccountConnected?.Invoke(this, e);
        }

        protected virtual void OnAccountDisconnected(AccountDisconnectedEventArgs e)
        {
            AccountDisconnected?.Invoke(this, e);

            AppKit.EventsController.SendEvent(new Event
            {
                name = "DISCONNECT_SUCCESS"
            });
        }

        protected virtual void OnAccountChanged(AccountChangedEventArgs e)
        {
            AccountChanged?.Invoke(this, e);
        }

        protected virtual void OnChainChanged(ChainChangedEventArgs e)
        {
            ChainChanged?.Invoke(this, e);
        }

        protected abstract Task InitializeAsyncCore(AppKitConfig config, SignClientUnity signClient);

        protected abstract ConnectionProposal ConnectCore();

        protected abstract Task<bool> TryResumeSessionAsyncCore();

        protected abstract Task DisconnectAsyncCore();

        protected abstract Task ChangeActiveChainAsyncCore(Chain chain);

        protected abstract Task<Account> GetAccountAsyncCore();

        protected abstract Task<Account[]> GetAccountsAsyncCore();

        public class AccountConnectedEventArgs : EventArgs
        {
            public Func<Task<Account>> GetAccount { get; }
            public Func<Task<Account[]>> GetAccounts { get; }

            public AccountConnectedEventArgs(Func<Task<Account>> getAccount, Func<Task<Account[]>> getAccounts)
            {
                GetAccount = getAccount;
                GetAccounts = getAccounts;
            }
        }

        public class AccountDisconnectedEventArgs : EventArgs
        {
            public static AccountDisconnectedEventArgs Empty { get; } = new();
        }

        public class AccountChangedEventArgs : EventArgs
        {
            public Account Account { get; }

            public AccountChangedEventArgs(Account account)
            {
                Account = account;
            }
        }

        public class ChainChangedEventArgs : EventArgs
        {
            public string ChainId { get; }

            public ChainChangedEventArgs(string chainId)
            {
                ChainId = chainId;
            }
        }
    }

    public enum ConnectorType
    {
        None,
        WalletConnect,
        WebGl
    }
}