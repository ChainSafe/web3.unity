using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventEmitter.NET;
using EventEmitter.NET.Interfaces;
using MetaMask.Cryptography;
using MetaMask.Logging;
using MetaMask.Models;
using MetaMask.Sockets;
using MetaMask.Transports;

using Newtonsoft.Json;
using evm.net;
using evm.net.Models;

namespace MetaMask
{

    /// <summary>
    /// The main interface to interact with the MetaMask wallet.
    /// </summary>
    public class MetaMaskWallet : IProvider, IEvents, IMetaMaskEventsHandler, IDisposable
    {
        public const string Version = "1.2.2";
        public const int AppIconMaxSize = 10_000;
        
        private static readonly JsonConverter[] Converters = {
            new BigIntegerHexConverter()
        };

        #region Events

        public bool Connecting => this.connectionTcs != null && !this.connectionTcs.Task.IsCompleted;

        public IMetaMaskEventsHandler Events => this;

        public EventHandler<MetaMaskConnectEventArgs> StartConnectingHandler { get; set; }

        /// <summary>Raised when the wallet is ready.</summary>
        public EventHandler WalletReadyHandler { get; set; }
        /// <summary>Raised when the wallet is paused.</summary>
        public EventHandler WalletPausedHandler { get; set; }

        private event EventHandler _walletConnectedHandler;

        /// <summary>Occurs when a wallet is connected.</summary>
        public EventHandler WalletConnectedHandler
        {
            get => _walletConnectedHandler;
            set => _walletConnectedHandler = value;
        }

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

        #endregion

        #region Constants

        /// <summary>The URL of the MetaMask app.</summary>
        public const string MetaMaskUniversalLinkUrl = "https://metamask.app.link";
        public const string MetaMaskDeepLinkUrl = "metamask://";

        /// <summary>The URL of the socket.io server.</summary>
        public const string SocketUrl = "https://metamask-sdk-socket.metafi.codefi.network";

        /// <summary>The name of the event that is fired when a message is received.</summary>
        protected const string MessageEventName = "message";
        /// <summary>The name of the event that is raised when a user joins a channel.</summary>       
        protected const string JoinChannelEventName = "join_channel";
        /// <summary>The name of the event that is fired when the user leaves a channel.</summary>       
        protected const string LeaveChannelEventName = "leave_channel";
        /// <summary>The name of the event that is fired when a client connects.</summary>
        protected const string ClientsConnectedEventName = "clients_connected";
        /// <summary>The name of the event that is raised when clients are disconnected.</summary>
        protected const string ClientsDisconnectedEventName = "clients_disconnected";
        /// <summary>The name of the event that is raised when clients are waiting to join.</summary>
        protected const string ClientsWaitingToJoinEventName = "clients_waiting_to_join";

        protected const string ClientsReadyEventName = "clients_ready";

        protected const string TrackingEventConnectionStarted = "sdk_connect_request_started";
        protected const string TrackingEventReconnectionStarted = "sdk_reconnect_request_started";
        protected const string TrackingEventConnected = "sdk_connection_established";
        protected const string TrackingEventConnectionAuthorized = "sdk_connection_authorized";
        protected const string TrackingEventConnectionRejected = "sdk_connection_rejected";
        protected const string TrackingEventDisconnected = "sdk_disconnected";

        #endregion

        #region Fields
        /// <summary>List of methods that should be redirected.</summary>
        protected static List<string> MethodsToRedirect = new List<string>() {
            "eth_sendTransaction",
            "eth_signTransaction",
            "eth_sign",
            "personal_sign",
            "eth_signTypedData",
            "eth_signTypedData_v3",
            "eth_signTypedData_v4",
            "wallet_watchAsset",
            "wallet_addEthereumChain",
            "wallet_switchEthereumChain"
        };
        
        protected static List<string> MethodsToSendToFallback = new List<string>() {
            "eth_call",
            "eth_blockNumber",
            "eth_estimateGas",
            "eth_gasPrice",
            "eth_getBlockByNumber",
            "eth_getBlockTransactionCountByHash",
            "eth_getBlockTransactionCountByNumber",
            "eth_getCode",
            "eth_getTransactionByBlockHashAndIndex",
            "eth_getTransactionByHash",
            "eth_getTransactionCount",
            "eth_getTransactionReceipt",
        };
        /// <summary>The users wallet session.</summary>
        protected MetaMaskSession session;
        /// <summary>The transport used in the wallet session.</summary>
        protected IMetaMaskTransport transport;
        /// <summary>The socket connection used in the user wallet session.</summary>
        protected IMetaMaskSocketWrapper socket;
        /// <summary>The socket connection url.</summary>
        protected string socketUrl;

        /// <summary>
        /// The event delegator responsible for emitting events relating to JSON requests
        /// </summary>
        protected EventDelegator _eventDelegator;

        /// <summary>
        /// The data manager to use to persist session data
        /// </summary>
        protected MetaMaskDataManager _dataManager;

        /// <summary>Indicates whether the keys have been exchanged.</summary>
        /// <returns>True if the keys have been exchanged; otherwise, false.</returns>
        protected bool keysExchanged = false;
        protected bool handshakeCompleted = false;
        /// <summary>Gets or sets the selected address.</summary>
        protected string selectedAddress = string.Empty;
        /// <summary>The ID of the chain that is currently selected.</summary>
        protected string selectedChainId = string.Empty;

        /// <summary>
        /// The public key of the wallet that is currently connected.
        /// </summary>
        protected string walletPublicKey = string.Empty;

        /// <summary>Indicates whether the application is connected to the Internet.</summary>
        /// <returns>True if the application is connected to the Internet; otherwise, false.</returns>
        protected bool connected = false;

        protected bool socketConnected = false;
        /// <summary>Gets or sets a value indicating whether the application is paused.</summary>
        /// <value>true if the application is paused; otherwise, false.</value>
        protected bool paused = false;

        protected bool authorized = false;
        protected bool authorizing = false;

        protected TaskCompletionSource<string[]> connectionTcs;
        protected TaskCompletionSource<bool> validateKeyTcs;

        /// <summary>The Socket URL</summary>
        protected string universalConnectionUrl;
        protected string deeplinkConnectionUrl;

        /// <summary>Submitted requests dictionary.</summary>
        protected Dictionary<string, MetaMaskSubmittedRequest> submittedRequests = new Dictionary<string, MetaMaskSubmittedRequest>();

        protected string analyticsPlatform = "unknown";

        protected ConnectionContext _connectionContext;

        protected string sessionId = "metamask.session.data";

        protected bool sessionEnded;

        protected bool isResume;

        protected Queue<object> queuedMessage = new Queue<object>();

        protected IProvider fallbackProvider;
        #endregion

        #region Properties

        /// <summary>The MetaMask session.</summary>
        public MetaMaskSession Session => this.session;

        /// <summary>Gets or sets the transport used to send and receive data.</summary>
        /// <value>The transport used to send and receive data.</value>
        public IMetaMaskTransport Transport
        {
            get => this.transport;
            set => this.transport = value;
        }

        /// <summary>Gets or sets the socket.</summary>
        /// <value>The socket.</value>
        public IMetaMaskSocketWrapper Socket
        {
            get => this.socket;
            set => this.socket = value;
        }

        public IProvider FallbackProvider
        {
            get => this.fallbackProvider;
            set => this.fallbackProvider = value;
        }

        public string UserAgent { get; set; } = "UnityUGUITransport/1.0.0";

        public DateTime LastActive => this.session.LastActive;

        /// <summary>Gets the currently selected address.</summary>
        /// <returns>The currently selected address.</returns>
        public string SelectedAddress => this.selectedAddress;
        
        /// <summary>
        /// Gets the currently selected chain as a long.
        /// </summary>
        public long ChainId => !string.IsNullOrWhiteSpace(this.selectedChainId) ? Convert.ToInt32(this.selectedChainId, 16) : 0x0;

        /// <summary>Gets the ID of the currently selected chain.</summary>
        /// <returns>The ID of the currently selected chain.</returns>
        public string SelectedChainId => this.selectedChainId;

        /// <summary>Gets the public key of the wallet.</summary>
        /// <returns>The public key of the wallet.</returns>
        public string WalletPublicKey => this.walletPublicKey;

        /// <summary>Gets a value indicating whether the client is connected to the server.</summary>
        /// <returns>true if the client is connected to the server; otherwise, false.</returns>
        public bool IsConnected => this.connected;

        public string AppIcon => $"data:image/png;base64,{this.session.Data.AppIcon}";

        public MetaMaskOriginatorInfo OriginatorInfo =>
            new()
            {
                Title = this.session.Data.AppName,
                Url = this.session.Data.AppUrl,
                // Source = this.analyticsPlatform, TODO Figure out what source to default to
            };

        /// <summary>Gets a value indicating whether the application is paused.</summary>
        /// <returns>true if the application is paused; otherwise, false.</returns>
        public bool IsPaused => this.paused;

        public bool IsAuthorized => this.authorized;

        public bool HasSession => _dataManager != null && !string.IsNullOrWhiteSpace(sessionId) &&
                                  _dataManager.Storage.Exists(sessionId);

        /// <summary>Gets or sets the analytics platform.</summary>
        public string AnalyticsPlatform
        {
            get
            {
                return this.analyticsPlatform;
            }
            set
            {
                this.analyticsPlatform = value;
            }
        }

        /// <summary>
        /// The context in which this MetaMaskWallet connection is made
        /// </summary>
        public ConnectionContext ConnectionContext
        {
            get
            {
                return _connectionContext;
            }
        }

        #endregion

        #region Constructors

        public MetaMaskWallet(MetaMaskDataManager dataManager, IAppConfig appConfig,
            string sessionId, IEciesProvider eciesProvider, IMetaMaskTransport transport, 
            IMetaMaskSocketWrapper socket, string socketUrl = SocketUrl)
        {
            this.sessionId = sessionId;
            this._dataManager = dataManager;

            LoadOrCreateSession(appConfig, eciesProvider);

            this.transport = transport;
            this.socket = socket;
            this.socketUrl = socketUrl;

            this.socket.Connected += OnSocketConnected;
            this.socket.Disconnected += OnSocketDisconnected;
        }

        /// <summary>Creates a new instance of the MetaMaskWallet class.</summary>
        /// <param name="session">The MetaMask session.</param>
        /// <param name="transport">The MetaMask transport.</param>
        /// <param name="socket">The MetaMask socket.</param>
        /// <param name="socketUrl">The MetaMask socket URL.</param>
        public MetaMaskWallet(MetaMaskSession session, IMetaMaskTransport transport, IMetaMaskSocketWrapper socket, string socketUrl = SocketUrl)
        {
            this.session = session;
            this.transport = transport;
            this.socket = socket;
            this.socketUrl = socketUrl;

            this.socket.Connected += OnSocketConnected;
            this.socket.Disconnected += OnSocketDisconnected;
        }

        #endregion

        #region Protected Methods

        protected void ReloadNewSession()
        {
            LoadOrCreateSession(session.Data, session.EciesProvider);
        }

        protected void LoadOrCreateSession(IAppConfig appConfig, IEciesProvider eciesProvider)
        {
            var sessionData = new MetaMaskSessionData(appConfig);
            this._dataManager.LoadInto(this.sessionId, sessionData);
            this.session = new MetaMaskSession(eciesProvider, sessionData);
        }

        /// <summary>Sends a message to the other party.</summary>
        /// <param name="data">The data to send.</param>
        /// <param name="encrypt">Whether to encrypt the data.</param>
        protected void SendMessage(object data, bool encrypt)
        {
            if (this.paused)
            {
                MetaMaskDebug.Log("Queuing message");
                queuedMessage.Enqueue(data);
                /*void SendMessageWhenReady(object sender, EventArgs e)
                {
                    // grab "this" from sender, probably more memory safe
                    var senderThis = (MetaMaskWallet)sender;
                    MetaMaskDebug.Log("Sending queued message");
                    senderThis.socket.Emit(MessageEventName, message);
                    WalletReady -= SendMessageWhenReady;
                }
                WalletReady += SendMessageWhenReady;*/
            }
            else
            {
                var message = this.session.PrepareMessage(data, encrypt, this.WalletPublicKey);
                MetaMaskDebug.Log("Sending message");
                this.socket.Emit(MessageEventName, message);
            }
        }

        protected void EmptyMessageQueue()
        {
            while (queuedMessage.Count > 0)
            {
                var message = queuedMessage.Dequeue();
                if (message == null)
                {
                    MetaMaskDebug.LogWarning("Message in queue is null! Perhaps this object was collected by GC?");
                    continue;
                }
                
                MetaMaskDebug.Log("Sending queued message");
                SendMessage(message, true);
                MetaMaskDebug.Log("Queued message sent!");
            }
            
            queuedMessage.Clear();
        }

        /// <summary>Sends analytics data to Socket.io server.</summary>
        /// <param name="eventId">The event to send to analytics</param>
        public async void SendAnalytics(string eventId)
        {
            MetaMaskOriginatorInfo originatorInfo = OriginatorInfo;
            if (eventId == "sdk_connect_request_started")
            {
                if (AppIcon.Length <= AppIconMaxSize)
                    originatorInfo.Icon = AppIcon;
                else
                    MetaMaskDebug.LogWarning("The app icon provided is over the 8k limit. Excluding app icon");
            }

            var analyticsInfo = new MetaMaskAnalyticsInfo
            {
                Id = this.session.Data.ChannelId,
                Event = eventId,
                OriginatorInfo = originatorInfo,
                Platform = this.analyticsPlatform,
            };

            string jsonString = JsonConvert.SerializeObject(analyticsInfo);
            MetaMaskDebug.Log("Sending Analytics: " + jsonString);

            var url = this.socketUrl.EndsWith("/") ? this.socketUrl + "debug" : this.socketUrl + "/debug";

            var response = await this.socket.SendWebRequest(url, jsonString,
                new Dictionary<string, string> {{"Content-Type", "application/json"}});
            if (response.IsSuccessful)
            {
                var r = JsonConvert.DeserializeObject<AnalyticsResponse>(response.Response);
                if (r != null && r.Success) return;
            }

            MetaMaskDebug.LogWarning("Sending analytics has failed:");
            MetaMaskDebug.LogWarning(response.Response);
            MetaMaskDebug.LogWarning(response.Error);
        }

        /// <summary>Sends the originator information to the clipboard.</summary>
        protected void SendOriginatorInfo()
        {
            var info = OriginatorInfo;
            var requestInfo = new MetaMaskRequestInfo
            {
                Type = "originator_info",
                OriginatorInfo = info
            };
            SendMessage(requestInfo, true);
        }

        /// <summary>Called when the wallet is paused.</summary>
        protected void OnWalletPaused()
        {
            if (this.paused)
                return;
            
            MetaMaskDebug.Log("Wallet Paused");
            this.paused = true;

            WalletPausedHandler?.Invoke(this, null);
            
            // Re-save the connection urls with redirect
            /*if (transport.IsMobile)
            {
                SaveConnectionUrl(true);
            }*/

            SaveSession();
        }
        
        /// <summary>
        /// A function that completes when the wallet has successfully exchanged keys. This occurs
        /// when a valid response is gotten from the wallet. 
        /// </summary>
        protected async Task ValidateKeyExchange()
        {
            MetaMaskDebug.Log("Key validation requested");
            // If we already have a task waiting for a reply
            if (this.validateKeyTcs != null)
            {
                // Join the line, and wait for the original task
                MetaMaskDebug.Log("Key validation already pending");
                await this.validateKeyTcs.Task;
                return;
            }

            // This function will return a task when the key has been validated.
            // We use the connectionTcs to validate that the response was received.
            // We complete the validateKeyTcs once connectionTcs completes (inside KeyValidationComplete)
            this.validateKeyTcs = new TaskCompletionSource<bool>();
            var request = new MetaMaskEthereumRequest
            {
                Method = "eth_requestAccounts",
                Parameters = new string[] { }
            };
            string id = Guid.NewGuid().ToString();

            var submittedRequest = new MetaMaskSubmittedRequest
            {
                Method = request.Method,
            };
            
            // Listen for an error response for this request
            _eventDelegator.ListenForOnce<MetaMaskTypedDataMessage<JsonRpcError>>($"{id}-error", (sender, @event) =>
            {
                // Set exception to connection task
                this.connectionTcs.TrySetException(new Exception(@event.EventData.Data.Error.Message));
            });
            
            // Listen for a valid response for this request
            _eventDelegator.ListenForOnce<string>($"{id}-result", (sender, @event) =>
            {
                // Deserialize the response and complete the connection task
                var data =
                    JsonConvert.DeserializeObject<MetaMaskTypedDataMessage<JsonRpcResult<string[]>>>(@event.EventData, Converters);
                this.connectionTcs.TrySetResult(data.Data.Result);
            });

            // Send it
            MetaMaskDebug.Log("Sending eth_requestAccounts for key validation");
            this.submittedRequests.Add(id, submittedRequest);
            SendEthereumRequest(id, request, false);
            
            try
            {
                // Wait for the connection task to complete (above)
                await this.connectionTcs.Task;
                MetaMaskDebug.Log("Response for eth_requestAccounts during key validation");
                
                // If (somehow) someone else already completed this task
                if (this.validateKeyTcs.Task.IsCompleted)
                {
                    // Log a warning, possible race condition.
                    MetaMaskDebug.LogWarning("Response for eth_requestAccounts during key validation, but validation task already completed!");
                    return;
                }

                // Attempt to complete the validation task (everyone else waiting for this task at start of function)
                this.validateKeyTcs.TrySetResult(this.connectionTcs.Task.IsCompletedSuccessfully);
            }
            catch (Exception e)
            {
                // Let everyone else know the task failed with the original exception
                this.validateKeyTcs.SetException(e);
                MetaMaskDebug.Log("Key validation completed failed");
                return;
            }
        }

        /// <summary>Called when the wallet is ready.</summary>
        protected async Task OnWalletAuthorized()
        {
            MetaMaskDebug.Log("Wallet Ready Post pre-start");
            if (this.authorizing)
                return;

            this.authorizing = true;
            MetaMaskDebug.Log("Wallet Ready");
            this.paused = false;

            try
            {
                this.WalletConnectedHandler?.Invoke(this, EventArgs.Empty);
                await this.ValidateKeyExchange();
                MetaMaskDebug.Log("Key validation completed successfully");

                if (!this.authorized)
                {
                    SendAnalytics(TrackingEventConnectionAuthorized);
                    
                    MetaMaskDebug.Log("Invoking authorized event");
                    this.authorized = true;
                    this.WalletAuthorizedHandler?.Invoke(this, EventArgs.Empty);
                }

                if (!this.connected)
                {
                    MetaMaskDebug.Log("Invoking ready event");
                    this.connected = true;
                    this.WalletReadyHandler?.Invoke(this, EventArgs.Empty);

                    MetaMaskDebug.Log("Invoking transport onSuccess");
                    this.transport.OnSuccess();
                }
                MetaMaskDebug.Log("Resetting connection URL");
                // Resave connection URL in-case we added a redirect at some point
                this.SaveConnectionUrl();

                MetaMaskDebug.Log("Saving session");
                this.SaveSession();
                RequestChainId();

                // TODO Fix this, bug causes app to crash on Resume with queued messages
                //MetaMaskDebug.Log("Emptying message queue");
                this.EmptyMessageQueue();
            }
            catch (Exception e)
            {
                MetaMaskDebug.LogError(e);
                this.OnWalletUnauthorized();
            }
            finally
            {
                this.authorizing = false;
            }
        }

        /// <summary>Raised when the socket is connected.</summary>
        protected void OnSocketConnected(object sender, EventArgs e)
        {
            string channelId = this.session.Data.ChannelId;
            MetaMaskDebug.Log("Socket connected");
            MetaMaskDebug.Log("Channel ID: " + channelId);
            MetaMaskDebug.Log($"{MessageEventName}-{channelId}");

            // Listen for messages using the channel
            this.socket.On(MessageEventName, OnMessageReceived);
            this.socket.On($"{MessageEventName}-{channelId}", OnMessageReceived);
            this.socket.On($"{ClientsConnectedEventName}-{channelId}", OnClientsConnected);
            this.socket.On($"{ClientsDisconnectedEventName}-{channelId}", OnClientsDisconnected);
            this.socket.On($"{ClientsWaitingToJoinEventName}-{channelId}", OnClientsWaitingToJoin);

            this.socketConnected = true;
            this.socket.Disconnected += (o, args) => this.socketConnected = false;

            // Join the channel
            JoinChannel(channelId);
            
            // Alert transport we have connected
            transport.OnConnectRequest();
        }

        private void OnSocketDisconnected(object sender, EventArgs e)
        {
            MetaMaskDebug.Log("Socket disconnected");
        }

        protected void JoinChannel(string channelId)
        {
            MetaMaskDebug.Log("Joining channel");
            this.socket.Emit(JoinChannelEventName, channelId);
        }

        protected void LeaveChannel(string channelId)
        {
            MetaMaskDebug.Log("Leaving channel");
            this.socket.Emit(LeaveChannelEventName, channelId);
            
            SendAnalytics(TrackingEventDisconnected);
        }

        /// <summary>Called when a message is received.</summary>
        /// <param name="response">The response from the background task.</param>
        protected async void OnMessageReceived(string response)
        {
            MetaMaskDebug.Log("Message received");
            MetaMaskDebug.Log(response);
            
            // Update last active time through internal property setter
            this.session.Data.LastActive = DateTime.Now;
            
            if (response.StartsWith("{"))
                await HandleResponseWithTypes<MetaMaskMessage<KeyExchangeMessage>, MetaMaskMessage<string>>(response,
                    message => HandleKeyExchangeMessage(message.Message),
                    message => HandleEncryptedMessage(message.Message));
            else if (response.StartsWith("["))
                await HandleResponseWithTypes<MetaMaskMessage<KeyExchangeMessage>[], MetaMaskMessage<string>[]>(
                    response,
                    messages =>
                    {
                        // Key exchange & handshake
                        foreach (var message in messages)
                        {
                            HandleKeyExchangeMessage(message.Message);
                        }
                    }, messages => Task.WhenAll(messages.Select(m => HandleEncryptedMessage(m.Message)))
                );
        }

        protected async Task HandleResponseWithTypes<TKeyType, TEncrytpedType>(string json, Action<TKeyType> keyMessageCallback, Func<TEncrytpedType, Task> encryptedMessageCallback)
        {
            try
            {
                var keychainMessage = JsonConvert.DeserializeObject<TKeyType>(json, Converters);
                if (keychainMessage != null)
                {
                    keyMessageCallback(keychainMessage);
                }
            }
            catch (JsonSerializationException)
            {
                // If it wasn't a key exchange or handshake, try to decrypt the JSON
                try
                {
                    MetaMaskDebug.Log("Encrypted message received");
                    var encryptedMessages = JsonConvert.DeserializeObject<TEncrytpedType>(json, Converters);
                    if (encryptedMessages != null)
                    {
                        await encryptedMessageCallback(encryptedMessages);
                    }
                } 
                catch (Exception e)
                {
                    MetaMaskDebug.LogException(e);
                }
            }
            catch (Exception e)
            {
                MetaMaskDebug.LogException(e);
            }
        }

        protected async Task HandleEncryptedMessage(string encryptedJson)
        {
            string decryptedJson;
            try
            {
                decryptedJson = this.session.DecryptMessage(encryptedJson);
            }
            catch (Exception)
            {
                // If we can't decrypt, our keys may be outdated
                MetaMaskDebug.LogError("Could not decrypt message, restarting key exchange");
                this.handshakeCompleted = false;
                ExchangeKeys(true);
                throw;
            }

            MetaMaskDebug.Log(decryptedJson);
            if (string.IsNullOrWhiteSpace(decryptedJson))
            {
                // If we can't decrypt, our keys may be outdated
                MetaMaskDebug.LogError("Could not decrypt message, restarting key exchange");
                this.handshakeCompleted = false;
                ExchangeKeys(true);
                return;
            }

            var typedMessage = JsonConvert.DeserializeObject<TypedMessage>(decryptedJson, Converters);
            //var decryptedMessage = JsonDocument.Parse(decryptedJson).RootElement;
            var decryptedMessageType = typedMessage.Type;
            //var decryptedMessageType = decryptedMessage.TryGetProperty("type", out var type)
            //    ? type.ToString()
            //    : string.Empty;

            if (decryptedMessageType == "pause")
            {
                OnWalletPaused();
                return;
            }
            else if (decryptedMessageType == "otp")
            {
                var answer = JsonConvert.DeserializeObject<OtpAnswerMessage>(decryptedJson, Converters).OtpAnswer;
                // var answer = decryptedMessage.TryGetProperty("otpAnswer", out var answerTypeElement)
                //    ? int.Parse(answerTypeElement.ToString())
                //    : throw new ArgumentException("Could not parse otp answer");

                OnOtpReceived(answer);
                return;
            }
            else if (decryptedMessageType == "ready")
            {
                // Key exchange successful
                this.handshakeCompleted = true;
                
                // If completed the handshake, send some analytics
                SendAnalytics(TrackingEventConnected);
                
                // Test key exchange by sending eth_requestAccounts
                await ValidateKeyExchange();
                if (!transport.IsMobile) return;
                    
                await OnWalletAuthorized();
                return;
            }

            if (!this.connected)
            {
                if (decryptedMessageType == "authorized")
                {
                    await OnWalletAuthorized();
                    return;
                }

                if (decryptedMessageType == "wallet_info")
                {
                    // TODO Store wallet info
                    // OnWalletAuthorized();
                    return;
                }

                if (decryptedMessageType == "terminate")
                {
                    OnWalletUnauthorized();
                    return;
                }
            }

            var dataMessage = JsonConvert.DeserializeObject<MetaMaskDataMessage>(decryptedJson, Converters);
            if (dataMessage.Data != null)
            {
                var payload = dataMessage.Data;
                if (payload.IsResponse)
                {
                    OnEthereumRequestReceived(payload, decryptedJson);
                }
                else
                {
                    OnEthereumEventReceived(payload, decryptedJson);
                }
            }
        }

        protected void HandleKeyExchangeMessage(KeyExchangeMessage message)
        {
            var messageType = message.Type;
            
            if (messageType == "key_handshake_start")
            {
                // Always restart key exchange when we get key_handshake_start
                // regardless of previous key exchange
                this.keysExchanged = false;
                this.handshakeCompleted = false;
                this.paused = false;
                this.connected = false;
                this.sessionEnded = false;
                
                ClearValidateTask();

                ExchangeKeys();
                return;
            }

            if (!this.handshakeCompleted && messageType == "key_handshake_SYNACK")
            {
                MetaMaskDebug.Log("Wallet public key");
                this.walletPublicKey = message.PublicKey;
                MetaMaskDebug.Log(this.WalletPublicKey);
                var keyExchangeACK = new KeyExchangeMessage("key_handshake_ACK", this.session.PublicKey);

                SendMessage(keyExchangeACK, false);
                SendOriginatorInfo();
                return;
            }

            if (messageType == "ping")
            {
                MetaMaskDebug.Log("Wallet responded with ping");
                //MetaMaskDebug.LogError("Connection failed, prompting user to try again");
                // Try again?
                // EndSession(true);

                // Exchange keys again?
                ExchangeKeys(true);
                
                // Ping?
                // SendMessage(new MetaMaskPing(), false);
                return;
            }
        }

        protected void OnOtpReceived(int answer)
        {
            MetaMaskDebug.Log($"Displaying OTP Answer");
            
            this.transport.OnOTPCode(answer);
        }

        /// <summary>Called when the clients are waiting to join.</summary>
        /// <param name="response">The response sent by the server.</param>
        protected void OnClientsWaitingToJoin(string response)
        {
            MetaMaskDebug.Log("Clients waiting to join");
        }

        /// <summary>Called when the server sends a response to the client's connection request.</summary>
        /// <param name="response">The response sent by the server.</param>
        protected void OnClientsConnected(string response)
        {
            MetaMaskDebug.Log("Clients connected");

             SendAnalytics(isResume ? TrackingEventReconnectionStarted : TrackingEventConnectionStarted);

            if (this.paused)
            {
                MetaMaskDebug.Log("Wallet Un-paused");
                this.paused = false;

                /*
                if (this.transport.IsMobile)
                {
                    // Re-save the connection urls with no redirect
                    SaveConnectionUrl();
                }
                */
                
                //WalletReady?.Invoke(this, EventArgs.Empty);
            }
            
            
            //ExchangeKeys();
        }

        protected void ExchangeKeys(bool forceExchange = false)
        {
            if (!this.keysExchanged || forceExchange)
            {
                MetaMaskDebug.Log("Exchanging keys");
                var keyExchangeSYN = new KeyExchangeMessage("key_handshake_SYN", this.session.PublicKey);
                SendMessage(keyExchangeSYN, false);
                this.keysExchanged = true;
            }
        }

        /// <summary>Called when the server sends a response to the client's disconnection request.</summary>
        /// <param name="response">The response sent by the server.</param>
        protected void OnClientsDisconnected(string response)
        {
            MetaMaskDebug.Log("Clients disconnected");
            
            SendAnalytics(TrackingEventDisconnected);

            if (!this.paused)
            {
                OnWalletPaused();
            }
        }

        protected void OnWalletUnauthorized()
        {
            this.authorized = false;
            SendAnalytics(TrackingEventConnectionRejected);
            WalletUnauthorizedHandler?.Invoke(this, EventArgs.Empty);
            EndSession();
        }

        protected void RequestChainId()
        {
            var ethChainId = new MetaMaskEthereumRequest()
            {
                Method = "eth_chainId",
                Parameters = new object[] { }
            };

            Request(ethChainId);
        }

        /// <summary>Raised when an Ethereum request is received.</summary>
        /// <param name="id">The request ID.</param>
        /// <param name="data">The request data.</param>
        protected void OnEthereumRequestReceived(JsonRpcPayload payload, string json)
        {
            var id = payload.Id?.ToString();
            MetaMaskSubmittedRequest request = null;
            if (id != null && this.submittedRequests.ContainsKey(id))
            {
                request = this.submittedRequests[id];
            }

            // The request has failed with an error
            if (payload.IsError)
            {
                var errorPayload = JsonConvert.DeserializeObject<MetaMaskTypedDataMessage<JsonRpcError>>(json, Converters);
                var ex = new Exception(errorPayload.Data.Error.Code.ToString());
                _eventDelegator.Trigger($"{id}-error", errorPayload);
                this.transport.OnFailure(ex);
                //request?.Promise.SetException(ex);
                EthereumRequestFailedHandler?.Invoke(this, new MetaMaskEthereumRequestFailedEventArgs(request, errorPayload.Data));
            }

            // The request has been successful
            else if (payload.IsResponse && request != null)
            {
                switch (request.Method)
                {
                    case "metamask_getProviderState":
                        var providerState =
                            JsonConvert.DeserializeObject<MetaMaskTypedDataMessage<ProviderState>>(json, Converters);
                        OnAccountsChanged(providerState.Data.Accounts);
                        OnChainIdChanged(new ChainData()
                        {
                            ChainId = providerState.Data.ChainId
                        });
                        break;
                    case "eth_requestAccounts":
                        var accounts = JsonConvert
                            .DeserializeObject<MetaMaskTypedDataMessage<JsonRpcResult<string[]>>>(json, Converters).Data.Result;
                        OnAccountsChanged(accounts);
                        break;
                    case "eth_chainId":
                        var chainId = JsonConvert
                            .DeserializeObject<MetaMaskTypedDataMessage<JsonRpcResult<string>>>(json, Converters).Data.Result;
                        OnChainIdChanged(new ChainData()
                        {
                            ChainId = chainId
                        });
                        break;
                }
                MetaMaskDebug.Log("Setting result of request task");
                _eventDelegator.Trigger($"{id}-result", json);
                //request?.Promise.SetResult(result);
                EthereumRequestResultReceivedHandler?.Invoke(this, new MetaMaskEthereumRequestResultEventArgs(request, json));
            }
        }

        /// <summary>Handles the event that is fired when an Ethereum event is received.</summary>
        /// <param name="data">The event data.</param>
        protected void OnEthereumEventReceived(JsonRpcPayload payload, string json)
        {
            var method = payload.Method;
            switch (method)
            {
                case "metamask_accountsChanged":
                    var accounts = JsonConvert.DeserializeObject<MetaMaskTypedDataMessage<JsonRpcRequest<string[]>>>(json, Converters).Data.Parameters;
                    OnAccountsChanged(accounts, !connected);
                    break;
                case "metamask_chainChanged":
                    var chainData =
                        JsonConvert.DeserializeObject<MetaMaskTypedDataMessage<JsonRpcRequest<ChainData>>>(json, Converters).Data.Parameters;
                    OnChainIdChanged(chainData);
                    break;
            }
        }

        /// <summary>Handles the event that is fired when an Account changed event is received.</summary>
        /// <param name="data">The event data.</param>
        protected void OnAccountsChanged(string[] accounts, bool triggerConnectionTask = false)
        {
            MetaMaskDebug.Log("Account changed");
            try
            {
                this.selectedAddress = accounts[0].ToString();
                AccountChangedHandler?.Invoke(this, EventArgs.Empty);

                if (triggerConnectionTask && this.connectionTcs != null)
                {
                    MetaMaskDebug.Log("Setting result of connection task");
                    this.connectionTcs.SetResult(accounts);
                }
            }
            catch
            {
                this.selectedAddress = string.Empty;
            }
        }

        /// <summary>Handles the event that is fired when an Chain ID changed event is received.</summary>
        /// <param name="data">The event data.</param>
        protected void OnChainIdChanged(ChainData newChainId)
        {
            MetaMaskDebug.Log("Chain ID changed");
            this.selectedChainId = newChainId.ChainId;
            ChainIdChangedHandler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Sends an Ethereum request to the MetaMask server.</summary>
        /// <param name="id">The request ID.</param>
        /// <param name="request">The request to send.</param>
        /// <param name="openTransport">Whether to open the transport if it isn't already open.</param>
        protected void SendEthereumRequest(string id, MetaMaskEthereumRequest request, bool openTransport)
        {
            request.Id = id;
            MetaMaskDebug.Log("Sending a new request");
            MetaMaskDebug.Log(JsonConvert.SerializeObject(request));

            SendMessage(request, true);

            if (openTransport)
            {
                try
                {
                    this.transport.OnRequest(id, request);
                }
                catch
                {
                    // Ignore the exception as some transport may not implement this method
                }
            }
        }

        /// <summary>Determines whether the specified method should open the MetaMask app.</summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the method should open the MetaMask app; otherwise, false.</returns>
        protected bool ShouldOpenMM(string method)
        {

            // Only open the wallet for requesting accounts when the address is not already provided.
            if (method == "eth_requestAccounts" && string.IsNullOrEmpty(this.selectedAddress))
            {
                return true;
            }

            if (MethodsToRedirect.Contains(method))
            {
                return true; 
            } 
            else if (this.paused && transport.IsMobile)
            {
                // Set a connection redirect URL so we can open deeplink for this method
                SaveConnectionUrl(true);
                return true;
            }

            return false;
        }

        #endregion

        #region Public Methods

        /// <summary>Sends a request to the MetaMask server.</summary>
        /// <param name="request">The request to send.</param>
        /// <returns>The response from the server.</returns>
        public Task<TR> Request<TR>(MetaMaskEthereumRequest request)
        {
            // Check if the parameters passed in has a ISerializerCallback
            if (request.Parameters is ISerializerCallback callback)
            {
                // If it does, replace the parameters with the result of the callback
                // Store old value if callback result is null
                // fail here if callback fails, we don't want to silence errors
                var oldParm = request.Parameters;
                try
                {
                    request.Parameters = callback.OnSerialize() ?? oldParm;
                }
                catch (Exception e)
                {
                    MetaMaskDebug.LogException(e);
                    MetaMaskDebug.LogError($"FATAL: Serializer callback for type {oldParm.GetType().FullName} failed");
                    throw;
                }
            }
            
            // If we have a fallback provider, and this can be given to the fallback provider. Do that,
            // unless the method requires the wallet.
            if (FallbackProvider != null && MethodsToSendToFallback.Contains(request.Method))
            {
                var param = request.Parameters as object[] ?? new[] { request.Parameters };
                return FallbackProvider.Request<TR>(request.Method, param);
            }

            if (request.Method == "eth_requestAccounts" && !this.connected && typeof(TR) == typeof(string[]))
            {
                if (this.connectionTcs == null || this.connectionTcs.Task.IsCompleted || (this.connectionTcs.Task.IsCompleted && !this.connected))
                {
                    Connect();

                    if (this.connectionTcs == null)
                    {
                        throw new Exception("Failed to reconnect to socket during Request.");
                    }
                }
                return this.connectionTcs.Task as Task<TR>;
            }
            else if (!this.connected && !this.transport.IsMobile)
            {
                throw new Exception("MetaMask Wallet is not connected.");
            }
            else
            {
                if (!this.socketConnected)
                {
                    // Ensure we are marked as paused
                    if (!this.paused)
                        this.paused = true;
                    MetaMaskDebug.Log("Socket disconnected, reconnecting..");
                    // Connect the socket first before sending a message
                    Connect();
                    MetaMaskDebug.Log("Socket re-connected, resuming request sending");
                }
                
                // Send and deeplink if we're on mobile, wallet will auto-resume
                var tcs = new TaskCompletionSource<TR>();
                var id = Guid.NewGuid().ToString();
                var submittedRequest = new MetaMaskSubmittedRequest()
                {
                    Method = request.Method,
                };
                
                _eventDelegator.ListenForOnce<MetaMaskTypedDataMessage<JsonRpcError>>($"{id}-error", (sender, @event) =>
                {
                    var ex = new Exception(@event.EventData.Data.Error.Message);
                    tcs.TrySetException(ex);
                });
                
                _eventDelegator.ListenForOnce<string>($"{id}-result", (sender, @event) =>
                {
                    var result = JsonConvert.DeserializeObject<MetaMaskTypedDataMessage<JsonRpcResult<TR>>>(@event.EventData, Converters);
                    tcs.TrySetResult(result.Data.Result);
                });
                
                this.submittedRequests.Add(id, submittedRequest);
                SendEthereumRequest(id, request, this.socketConnected && ShouldOpenMM(request.Method));
                return tcs.Task;
            }
        }

        [Obsolete("This function should only be used internally or by editor code. You may want to use EndSession.")]
        public void ClearSession()
        {
            if (this._dataManager != null)
                this._dataManager.Delete(this.sessionId);

            // Recreate session
            ReloadNewSession();
        }

        private void ClearValidateTask()
        {
            MetaMaskDebug.Log("Clearing validation task");
            // Make sure we let everyone know, party's over :(
            // We're starting a new connect, so new validation is in order
            if (this.validateKeyTcs != null && !this.validateKeyTcs.Task.IsCompleted)
            {
                this.validateKeyTcs.SetCanceled();
            }

            this.validateKeyTcs = null;
        }

        /// <summary>Connects to the server.</summary>
        public void Connect()
        {
            MetaMaskDebug.Log("Connecting...");
            
            ReloadNewSession();
            
            this.connectionTcs = new TaskCompletionSource<string[]>();
            ClearValidateTask();

            isResume = !string.IsNullOrWhiteSpace(this.session.Data.ChannelId);
            if (!isResume)
                this.session.Data.ChannelId = Guid.NewGuid().ToString();
            
            if (_eventDelegator == null || _eventDelegator.Context != this.session.Data.ChannelId)
                _eventDelegator = new EventDelegator(this.session.Data.ChannelId);

            // Initialize the socket
            this.socket.Initialize(this.socketUrl, new MetaMaskSocketOptions()
            {
                ExtraHeaders = new Dictionary<string, string>
                {
                    {"User-Agent", this.UserAgent}
                }
            });
            this.socket.ConnectAsync();

            // Save connection urls in the transport
            SaveConnectionUrl();

            SaveSession();

            StartConnectingHandler?.Invoke(this, new MetaMaskConnectEventArgs(this.universalConnectionUrl, this.deeplinkConnectionUrl));
        }

        protected void SaveConnectionUrl(bool redirect = false)
        {
            string channelId = this.session.Data.ChannelId;
            var urlParams =
                $"channelId={Uri.EscapeDataString(channelId)}&pubkey={Uri.EscapeDataString(this.session.PublicKey)}&comm=socket";

            if (redirect)
                urlParams += "&redirect=true";
            
            this.universalConnectionUrl = $"{MetaMaskUniversalLinkUrl}/connect?{urlParams}";
            this.deeplinkConnectionUrl = $"{MetaMaskDeepLinkUrl}connect?{urlParams}";
            
            MetaMaskDebug.Log("Setting connection URLs: " + this.universalConnectionUrl);
            try
            {
                this.transport.UpdateUrls(universalConnectionUrl, deeplinkConnectionUrl);
            }
            catch (Exception exception)
            {
                MetaMaskDebug.LogError("Opening transport for connection has failed");
                MetaMaskDebug.LogException(exception);
            }
        }

        /// <summary>Disconnects the client from the server.</summary>
        public void Disconnect()
        {
            MetaMaskDebug.Log("Disconnected");
            LeaveChannel();
            
            this.transport.OnDisconnect();

            this.connected = false;
            this.connectionTcs = null;

            // Force reauthorization
            this.authorized = false;
            this.paused = false;
            this.keysExchanged = false;
            this.handshakeCompleted = false;

            this.walletPublicKey = string.Empty;
            
            this.selectedAddress = string.Empty;
            this.selectedChainId = string.Empty;

            this.socket.DisconnectAsync();
            WalletDisconnectedHandler?.Invoke(this, EventArgs.Empty);
            
            SaveSession();
        }

        /// <summary>Disposes and resets the wallet client when a user is disconnected.</summary>
        public void Dispose()
        {
            Disconnect();
            this.socket.Dispose();
            this._eventDelegator.Dispose();
        }

        private void LeaveChannel()
        {
            string channelId = this.session.Data.ChannelId;

            // Leave the channel
            LeaveChannel(channelId);
        }

        public void SaveSession()
        {
            if (this._dataManager != null && !sessionEnded)
            {
                this._dataManager.Save(this.sessionId, this.session.Data);
            }
        }

        #endregion
        
        // Used by evm.net
        public string ConnectedAddress
        {
            get
            {
                return SelectedAddress;
            }
        }
        
        public Task<object> Request(MetaMaskEthereumRequest request)
        {
            return this.Request<object>(request);
        }

        public object Request(string method, object[] parameters = null)
        {
            return this.Request<object>(new MetaMaskEthereumRequest()
            {
                Method = method,
                Parameters = parameters,
            });
        }

        public Task<TR> Request<TR>(string method, object[] parameters = null)
        {
            return this.Request<TR>(new MetaMaskEthereumRequest()
            {
                Method = method,
                Parameters = parameters,
            });
        }

        public void EndSession(bool forceDisconnect = false)
        {
            sessionEnded = true;
            
#pragma warning disable CS0618
            ClearSession();
#pragma warning restore CS0618
            
            if (IsConnected || forceDisconnect)
                Disconnect();
        }

        EventDelegator IEvents.Events
        {
            get => _eventDelegator;
        }
    }
}