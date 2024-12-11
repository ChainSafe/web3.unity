using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using Reown.AppKit.Unity.Http;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity
{
    public class AccountController : INotifyPropertyChanged
    {
        public bool IsInitialized { get; set; }

        public bool IsConnected
        {
            get => _connectorController.IsAccountConnected;
        }
        
        public string Address
        {
            get => _address;
            set => SetField(ref _address, value);
        }
        
        public string AccountId
        {
            get => _accountId;
            set => SetField(ref _accountId, value);
        }

        public string ChainId
        {
            get => _chainId;
            set => SetField(ref _chainId, value);
        }
        
        public string ProfileName
        {
            get => _profileName;
            set => SetField(ref _profileName, value);
        }

        public AccountAvatar ProfileAvatar
        {
            get => _profileAvatar;
            set => SetField(ref _profileAvatar, value);
        }
        
        public string Balance
        {
            get => _balance;
            set => SetField(ref _balance, value);
        }
        
        public string BalanceSymbol
        {
            get => _balanceSymbol;
            set => SetField(ref _balanceSymbol, value);
        }

        private ConnectorController _connectorController;
        private NetworkController _networkController;
        private BlockchainApiController _blockchainApiController;

        private readonly UnityHttpClient _httpClient = new();
        
        private string _address;
        private string _accountId;
        private string _chainId;
        
        private string _profileName;
        private AccountAvatar _profileAvatar;
        
        private string _balance;
        private string _balanceSymbol;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public async Task InitializeAsync(ConnectorController connectorController, NetworkController networkController, BlockchainApiController blockchainApiController)
        {
            if (IsInitialized)
                throw new Exception("Already initialized"); // TODO: use custom ex type
            
            _connectorController = connectorController ?? throw new ArgumentNullException(nameof(connectorController));
            _networkController = networkController ?? throw new ArgumentNullException(nameof(networkController));
            _blockchainApiController = blockchainApiController ?? throw new ArgumentNullException(nameof(blockchainApiController));

#if !UNITY_WEBGL || UNITY_EDITOR
            _connectorController.AccountConnected += ConnectorAccountConnectedHandler;
            _connectorController.AccountChanged += ConnectorAccountChangedHandler;
#endif
        }

        private async void ConnectorAccountConnectedHandler(object sender, Connector.AccountConnectedEventArgs e)
        {
            var account = await e.GetAccount();
            if (account.AccountId == AccountId)
                return;
            
            Address = account.Address;
            AccountId = account.AccountId;
            ChainId = account.ChainId;
            
            await Task.WhenAll(
                UpdateBalance(),
                UpdateProfile()
            );
        }

        private async void ConnectorAccountChangedHandler(object sender, Connector.AccountChangedEventArgs e)
        {
            var oldAddress = Address;

            Address = e.Account.Address;
            AccountId = e.Account.AccountId;
            ChainId = e.Account.ChainId;

            await Task.WhenAll(
                UpdateBalance(),
                e.Account.Address != oldAddress ? UpdateProfile() : Task.CompletedTask
            );
        }

        public async Task UpdateProfile()
        {
            if (string.IsNullOrWhiteSpace(Address))
                return;
            
            var identity = await _blockchainApiController.GetIdentityAsync(Address);
            ProfileName = string.IsNullOrWhiteSpace(identity.Name)
                ? Address.Truncate()
                : identity.Name;

            if (!string.IsNullOrWhiteSpace(identity.Avatar))
            {
                try
                {
                    var headers = await _httpClient.HeadAsync(identity.Avatar);
                    var avatarFormat = headers["Content-Type"].Split('/').Last();
                    ProfileAvatar = new AccountAvatar(identity.Avatar, avatarFormat);
                }
                catch (Exception e)
                {
                    ProfileAvatar = default;
                }
            }
            else

            {
                ProfileAvatar = default;
            }
        }

        public async Task UpdateBalance()
        {
            if (string.IsNullOrWhiteSpace(Address))
                return;
            
            var response = await _blockchainApiController.GetBalanceAsync(Address);
            
            if (response.Balances.Length == 0)
            {
                Balance = "0.000";
                BalanceSymbol = _networkController.ActiveChain.NativeCurrency.symbol;
                return;
            }
            
            var balance = Array.Find(response.Balances,x => x.chainId == ChainId && string.IsNullOrWhiteSpace(x.address));

            if (string.IsNullOrWhiteSpace(balance.quantity.numeric))
            {
                Balance = "0.000";
                BalanceSymbol = _networkController.ActiveChain.NativeCurrency.symbol;
            }
            else
            {
                Balance = balance.quantity.numeric;
                BalanceSymbol = balance.symbol;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        
    }

    public readonly struct AccountAvatar
    {
        public readonly string AvatarUrl;
        public readonly string AvatarFormat;

        public AccountAvatar(string avatarUrl, string avatarFormat)
        {
            AvatarUrl = avatarUrl;
            AvatarFormat = avatarFormat;
        }

        public bool IsEmpty
        {
            get => string.IsNullOrWhiteSpace(AvatarUrl);
        }
    }
}