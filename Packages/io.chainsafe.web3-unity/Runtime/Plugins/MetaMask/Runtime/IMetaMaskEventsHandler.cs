using System;
using System.Text.Json;
using evm.net.Models;
using MetaMask.Models;

namespace MetaMask
{
    public interface IMetaMaskEventsHandler : IMetaMaskEvents
    {
        public EventHandler<MetaMaskConnectEventArgs> StartConnectingHandler { get; set; }
        
        /// <summary>Raised when the wallet is ready.</summary>
        public EventHandler WalletReadyHandler { get; set; }
        /// <summary>Raised when the wallet is paused.</summary>
        public EventHandler WalletPausedHandler { get; set; }
        /// <summary>Occurs when a wallet is connected.</summary>
        public EventHandler WalletConnectedHandler { get; set; }
        /// <summary>Occurs when a wallet is disconnected.</summary>
        public EventHandler WalletDisconnectedHandler { get; set; }
        /// <summary>Occurs when the chain ID is changed.</summary>
        public EventHandler ChainIdChangedHandler { get; set; }
        /// <summary>Occurs when the account is changed.</summary>
        public EventHandler AccountChangedHandler { get; set; }
        /// <summary>Occurs when the wallet connection is authorized by the user.</summary>
        public EventHandler WalletAuthorizedHandler { get; set; }
        /// <summary>Occurs when the wallet connection is unauthorized by the user.</summary>
        public EventHandler WalletUnauthorizedHandler { get; set; }
        /// <summary>Occurs when the Ethereum request's response received.</summary>
        public EventHandler<MetaMaskEthereumRequestResultEventArgs> EthereumRequestResultReceivedHandler { get; set; }
        /// <summary>Occurs when the Ethereum request has failed.</summary>
        public EventHandler<MetaMaskEthereumRequestFailedEventArgs> EthereumRequestFailedHandler { get; set; }
        
        public event EventHandler<MetaMaskConnectEventArgs> StartConnecting
        {
            add => StartConnectingHandler += value;
            remove => StartConnectingHandler -= value;
        }

        /// <summary>Raised when the wallet is ready.</summary>
        public event EventHandler WalletReady
        {
            add => WalletReadyHandler += value;
            remove => WalletReadyHandler -= value;
        }
        
        /// <summary>Raised when the wallet is paused.</summary>
        public event EventHandler WalletPaused 
        {
            add => WalletPausedHandler += value;
            remove => WalletPausedHandler -= value;
        }
        
        /// <summary>Occurs when a wallet is connected.</summary>
        public event EventHandler WalletConnected
        {
            add => WalletConnectedHandler += value;
            remove => WalletConnectedHandler -= value;
        }
        
        /// <summary>Occurs when a wallet is disconnected.</summary>
        public event EventHandler WalletDisconnected
        {
            add => WalletDisconnectedHandler += value;
            remove => WalletDisconnectedHandler -= value;
        }
        
        /// <summary>Occurs when the chain ID is changed.</summary>
        public event EventHandler ChainIdChanged
        {
            add => ChainIdChangedHandler += value;
            remove => ChainIdChangedHandler -= value;
        }
        
        /// <summary>Occurs when the account is changed.</summary>
        public event EventHandler AccountChanged
        {
            add => AccountChangedHandler += value;
            remove => AccountChangedHandler -= value;
        }
        
        /// <summary>Occurs when the wallet connection is authorized by the user.</summary>
        public event EventHandler WalletAuthorized
        {
            add => WalletAuthorizedHandler += value;
            remove => WalletAuthorizedHandler -= value;
        }
        
        /// <summary>Occurs when the wallet connection is unauthorized by the user.</summary>
        public event EventHandler WalletUnauthorized
        {
            add => WalletUnauthorizedHandler += value;
            remove => WalletUnauthorizedHandler -= value;
        }
        
        /// <summary>Occurs when the Ethereum request's response received.</summary>
        public event EventHandler<MetaMaskEthereumRequestResultEventArgs> EthereumRequestResultReceived
        {
            add => EthereumRequestResultReceivedHandler += value;
            remove => EthereumRequestResultReceivedHandler -= value;
        }
        
        /// <summary>Occurs when the Ethereum request has failed.</summary>
        public event EventHandler<MetaMaskEthereumRequestFailedEventArgs> EthereumRequestFailed
        {
            add => EthereumRequestFailedHandler += value;
            remove => EthereumRequestFailedHandler -= value;
        }
    }
    
    public class MetaMaskEthereumRequestResultEventArgs : EventArgs
    {
        public readonly MetaMaskSubmittedRequest Request;
        public readonly string Result;

        /// <summary>Initializes a new instance of the <see cref="MetaMaskEthereumRequestResultEventArgs"/> class.</summary>
        /// <param name="request">The initial Ethereum request.</param>
        /// <param name="result">The request's result.</param>
        public MetaMaskEthereumRequestResultEventArgs(MetaMaskSubmittedRequest request, string result)
        {
            this.Request = request;
            this.Result = result;
        }
    }

    public class MetaMaskEthereumRequestFailedEventArgs : EventArgs
    {
        public readonly MetaMaskSubmittedRequest Request;
        public readonly JsonRpcError Error;

        /// <summary>Initializes a new instance of the <see cref="MetaMaskEthereumRequestFailedEventArgs"/> class.</summary>
        /// <param name="request">The initial Ethereum request.</param>
        /// <param name="error">The request's result.</param>
        public MetaMaskEthereumRequestFailedEventArgs(MetaMaskSubmittedRequest request, JsonRpcError error)
        {
            this.Request = request;
            this.Error = error;

        }
    }
    
    public class MetaMaskConnectEventArgs : EventArgs
    {
        /// <summary>The Url to be called</summary>
        public readonly string UniversalLink;
        public readonly string DeepLink;

        public MetaMaskConnectEventArgs(string universalLink, string deepLink)
        {
            UniversalLink = universalLink;
            DeepLink = deepLink;
        }
    }
}