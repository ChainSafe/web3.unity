﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using NativeWebSocket;
using MetaMask.SocketIOClient.JsonSerializer;
using MetaMask.SocketIOClient.Messages;
using MetaMask.SocketIOClient.Transport;
using MetaMask.SocketIOClient.UriConverters;
using UnityEngine.Networking;

namespace MetaMask.SocketIOClient
{
    /// <summary>
    /// socket.io client class
    /// </summary>
    public class SocketIO : IDisposable
    {
        /// <summary>
        /// Create SocketIO object with default options
        /// </summary>
        /// <param name="uri"></param>
        public SocketIO(string uri) : this(new Uri(uri)) { }

        /// <summary>
        /// Create SocketIO object with options
        /// </summary>
        /// <param name="uri"></param>
        public SocketIO(Uri uri) : this(uri, new SocketIOOptions()) { }

        /// <summary>
        /// Create SocketIO object with options
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        public SocketIO(string uri, SocketIOOptions options) : this(new Uri(uri), options) { }

        /// <summary>
        /// Create SocketIO object with options
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        public SocketIO(Uri uri, SocketIOOptions options)
        {
            ServerUri = uri ?? throw new ArgumentNullException("uri");
            Options = options ?? throw new ArgumentNullException("options");
            Initialize();
        }

        private Uri _serverUri;
        public Uri ServerUri
        {
            get => this._serverUri;
            set
            {
                if (this._serverUri != value)
                {
                    this._serverUri = value;
                    if (value != null && value.AbsolutePath != "/")
                    {
                        Namespace = value.AbsolutePath;
                    }
                }
            }
        }

        /// <summary>
        /// An unique identifier for the socket session. Set after the connect event is triggered, and updated after the reconnect event.
        /// </summary>
        public string Id { get; set; }

        public string Namespace { get; private set; }

        /// <summary>
        /// Whether or not the socket is connected to the server.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Gets current attempt of reconnection.
        /// </summary>
        public int Attempts { get; private set; }

        /// <summary>
        /// Whether or not the socket is disconnected from the server.
        /// </summary>
        public bool Disconnected => !Connected;

        public SocketIOOptions Options { get; }

        public IJsonSerializer JsonSerializer { get; set; }

        public IUriConverter UriConverter { get; set; }

        public Action UpdateLoop { get; private set; }

        public Action RunUpdateLoop { get; set; }

        //public HttpClient HttpClient { get; set; }

        public Func<IClientWebSocket> ClientWebSocketProvider { get; set; }
        private IClientWebSocket _clientWebsocket;

        public BaseTransport _transport;
        private List<Type> _expectedExceptions;
        private int _packetId;
        private bool _isConnectCoreRunning;
        private Uri _realServerUri;
        private Exception _connectCoreException;
        private Dictionary<int, Action<SocketIOResponse>> _ackHandlers;
        private List<OnAnyHandler> _onAnyHandlers;
        private Dictionary<string, Action<SocketIOResponse>> _eventHandlers;
        private CancellationTokenSource _connectionTokenSource;
        private double _reconnectionDelay;

        #region Socket.IO event
        public event EventHandler OnConnected;
        //public event EventHandler<string> OnConnectError;
        //public event EventHandler<string> OnConnectTimeout;
        public event EventHandler<string> OnError;
        public event EventHandler<string> OnDisconnected;

        /// <summary>
        /// Fired upon a successful reconnection.
        /// </summary>
        public event EventHandler<int> OnReconnected;

        /// <summary>
        /// Fired upon an attempt to reconnect.
        /// </summary>
        public event EventHandler<int> OnReconnectAttempt;

        /// <summary>
        /// Fired upon a reconnection attempt error.
        /// </summary>
        public event EventHandler<Exception> OnReconnectError;

        /// <summary>
        /// Fired when couldn’t reconnect within reconnectionAttempts
        /// </summary>
        public event EventHandler OnReconnectFailed;
        public event EventHandler OnPing;
        public event EventHandler<TimeSpan> OnPong;

        #endregion

        #region Observable Event
        //Subject<Unit> _onConnected;
        //public IObservable<Unit> ConnectedObservable { get; private set; }
        #endregion

        private void Initialize()
        {
            this._packetId = -1;
            this._ackHandlers = new Dictionary<int, Action<SocketIOResponse>>();
            this._eventHandlers = new Dictionary<string, Action<SocketIOResponse>>();
            this._onAnyHandlers = new List<OnAnyHandler>();

            //JsonSerializer = new SystemTextJsonSerializer();
            JsonSerializer = new Newtonsoft.Json.NewtonsoftJsonSerializer();
            UriConverter = new UriConverter();

            //HttpClient = new HttpClient();
            //ClientWebSocketProvider = () => new SystemNetWebSocketsClientWebSocket(Options.EIO);
            ClientWebSocketProvider = () => new NativeClientWebSocket(Options.EIO);
            this._expectedExceptions = new List<Type>
            {
                typeof(TimeoutException),
                typeof(WebSocketException),
                typeof(UnityWebRequestException),
                typeof(OperationCanceledException),
                typeof(TaskCanceledException)
            };
        }

        private async Task CreateTransportAsync()
        {
            Options.Transport = await GetProtocolAsync();
            if (Options.Transport == TransportProtocol.Polling)
            {
                HttpPollingHandler handler;
                if (Options.EIO == 3)
                    handler = new Eio3HttpPollingHandler();
                else
                    handler = new Eio4HttpPollingHandler();
                this._transport = new HttpTransport(handler, Options, JsonSerializer);
            }
            else
            {
                this._clientWebsocket = ClientWebSocketProvider();
                this._transport = new WebSocketTransport(this._clientWebsocket, Options, JsonSerializer);
            }
            this._transport.Namespace = Namespace;
            SetHeaders();
        }

        private void SetHeaders()
        {
            if (Options.ExtraHeaders != null)
            {
                foreach (var item in Options.ExtraHeaders)
                {
                    this._transport.AddHeader(item.Key, item.Value);
                }
            }
        }

        private void SyncExceptionToMain(Exception e)
        {
            this._connectCoreException = e;
            this._isConnectCoreRunning = false;
        }

        private void ConnectCore()
        {
            DisposeForReconnect();
            this._reconnectionDelay = Options.ReconnectionDelay;
            this._connectionTokenSource = new CancellationTokenSource();
            var cct = this._connectionTokenSource.Token;
            this._isConnectCoreRunning = true;
            this._connectCoreException = null;

            UpdateLoop = async () =>
            {
                while (true)
                {
                    this._clientWebsocket?.Dispose();
                    this._transport?.Dispose();
                    CreateTransportAsync().Wait();
                    this._realServerUri = UriConverter.GetServerUri(Options.Transport == TransportProtocol.WebSocket, ServerUri, Options.EIO, Options.Path, Options.Query);
                    try
                    {
                        if (cct.IsCancellationRequested)
                            break;
                        if (Attempts > 0)
                            OnReconnectAttempt?.Invoke(this, Attempts);
                        var timeoutCts = new CancellationTokenSource(Options.ConnectionTimeout);
                        this._transport.Subscribe(OnMessageReceived, OnErrorReceived);
                        await this._transport.ConnectAsync(this._realServerUri, timeoutCts.Token).ConfigureAwait(false);
                        break;
                    }
                    catch (Exception e)
                    {
                        if (this._expectedExceptions.Contains(e.GetType()))
                        {
                            if (!Options.Reconnection)
                            {
                                SyncExceptionToMain(e);
                                throw;
                            }
                            if (Attempts > 0)
                            {
                                OnReconnectError?.Invoke(this, e);
                            }
                            Attempts++;
                            if (Attempts <= Options.ReconnectionAttempts)
                            {
                                if (this._reconnectionDelay < Options.ReconnectionDelayMax)
                                {
                                    this._reconnectionDelay += 2 * Options.RandomizationFactor;
                                }
                                if (this._reconnectionDelay > Options.ReconnectionDelayMax)
                                {
                                    this._reconnectionDelay = Options.ReconnectionDelayMax;
                                }
                                await Task.Delay((int)this._reconnectionDelay).ConfigureAwait(false);
                            }
                            else
                            {
                                OnReconnectFailed?.Invoke(this, EventArgs.Empty);
                                break;
                            }
                        }
                        else
                        {
                            SyncExceptionToMain(e);
                            throw;
                        }
                    }
                }
                this._isConnectCoreRunning = false;
            };


            //var coroutine = ConnectCoroutine(cct);
            //SocketIOCouroutineRunner.Instance.RunCoroutine(coroutine);
            if (RunUpdateLoop != null)
            {
                RunUpdateLoop();
            }
            else
            {
                Task.Factory.StartNew(UpdateLoop);
            }
        }

        private async Task<TransportProtocol> GetProtocolAsync()
        {
            if (Options.Transport == TransportProtocol.Polling && Options.AutoUpgrade)
            {
                Uri uri = UriConverter.GetServerUri(false, ServerUri, Options.EIO, Options.Path, Options.Query);
                try
                {
                    var tcs = new TaskCompletionSource<string>();
                    UnityThread.executeCoroutine(GetProtocolCoroutine(uri.ToString(), tcs));
                    string text = await tcs.Task;
                    //string text = await HttpClient.GetStringAsync(uri);
                    if (text.Contains("websocket"))
                    {
                        return TransportProtocol.WebSocket;
                    }
                }
                catch { }
            }
            return Options.Transport;
        }

        private IEnumerator GetProtocolCoroutine(string uri, TaskCompletionSource<string> tcs)
        {
            var uwr = UnityWebRequest.Get(uri);
            yield return uwr.SendWebRequest();
            tcs.SetResult(uwr.downloadHandler.text);
        }

        public async Task ConnectAsync()
        {
            ConnectCore();
            while (this._isConnectCoreRunning)
            {
                await Task.Delay(20);
            }
            if (this._connectCoreException != null)
            {
                throw this._connectCoreException;
            }
        }

        private void PingHandler()
        {
            OnPing?.Invoke(this, EventArgs.Empty);
        }

        private void PongHandler(PongMessage msg)
        {
            OnPong?.Invoke(this, msg.Duration);
        }

        private void ConnectedHandler(ConnectedMessage msg)
        {
            Id = msg.Sid;
            Connected = true;
            OnConnected?.Invoke(this, EventArgs.Empty);
            if (Attempts > 0)
            {
                OnReconnected?.Invoke(this, Attempts);
            }
            Attempts = 0;
        }

        private void DisconnectedHandler()
        {
            InvokeDisconnect(DisconnectReason.IOServerDisconnect);
        }

        private void EventMessageHandler(EventMessage m)
        {
            var res = new SocketIOResponse(m.JsonElements, this)
            {
                PacketId = m.Id
            };
            foreach (var item in this._onAnyHandlers)
            {
                try
                {
                    item(m.Event, res);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            if (this._eventHandlers.ContainsKey(m.Event))
            {
                try
                {
                    this._eventHandlers[m.Event](res);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        private void AckMessageHandler(ClientAckMessage m)
        {
            if (this._ackHandlers.ContainsKey(m.Id))
            {
                var res = new SocketIOResponse(m.JsonElements, this);
                try
                {
                    this._ackHandlers[m.Id](res);
                }
                finally
                {
                    this._ackHandlers.Remove(m.Id);
                }
            }
        }

        private void ErrorMessageHandler(ErrorMessage msg)
        {
            OnError?.Invoke(this, msg.Message);
        }

        private void BinaryMessageHandler(BinaryMessage msg)
        {
            if (this._eventHandlers.ContainsKey(msg.Event))
            {
                try
                {
                    var response = new SocketIOResponse(msg.JsonElements, this)
                    {
                        PacketId = msg.Id
                    };
                    response.InComingBytes.AddRange(msg.IncomingBytes);
                    this._eventHandlers[msg.Event](response);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        private void BinaryAckMessageHandler(ClientBinaryAckMessage msg)
        {
            if (this._ackHandlers.ContainsKey(msg.Id))
            {
                try
                {
                    var response = new SocketIOResponse(msg.JsonElements, this)
                    {
                        PacketId = msg.Id,
                    };
                    response.InComingBytes.AddRange(msg.IncomingBytes);
                    this._ackHandlers[msg.Id](response);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        private void OnErrorReceived(Exception ex)
        {
            UnityEngine.Debug.LogException(ex);
            InvokeDisconnect(DisconnectReason.TransportClose);
        }

        private void OnMessageReceived(IMessage msg)
        {
            try
            {
                switch (msg.Type)
                {
                    case MessageType.Ping:
                        PingHandler();
                        break;
                    case MessageType.Pong:
                        PongHandler(msg as PongMessage);
                        break;
                    case MessageType.Connected:
                        ConnectedHandler(msg as ConnectedMessage);
                        break;
                    case MessageType.Disconnected:
                        DisconnectedHandler();
                        break;
                    case MessageType.EventMessage:
                        EventMessageHandler(msg as EventMessage);
                        break;
                    case MessageType.AckMessage:
                        AckMessageHandler(msg as ClientAckMessage);
                        break;
                    case MessageType.ErrorMessage:
                        ErrorMessageHandler(msg as ErrorMessage);
                        break;
                    case MessageType.BinaryMessage:
                        BinaryMessageHandler(msg as BinaryMessage);
                        break;
                    case MessageType.BinaryAckMessage:
                        BinaryAckMessageHandler(msg as ClientBinaryAckMessage);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task DisconnectAsync()
        {
            if (Connected)
            {
                var msg = new DisconnectedMessage
                {
                    Namespace = Namespace
                };
                try
                {
                    await this._transport.SendAsync(msg, CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
                InvokeDisconnect(DisconnectReason.IOClientDisconnect);
            }
        }

        /// <summary>
        /// Register a new handler for the given event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void On(string eventName, Action<SocketIOResponse> callback)
        {
            if (this._eventHandlers.ContainsKey(eventName))
            {
                this._eventHandlers.Remove(eventName);
            }
            this._eventHandlers.Add(eventName, callback);
        }



        /// <summary>
        /// Unregister a new handler for the given event.
        /// </summary>
        /// <param name="eventName"></param>
        public void Off(string eventName)
        {
            if (this._eventHandlers.ContainsKey(eventName))
            {
                this._eventHandlers.Remove(eventName);
            }
        }

        public void OnAny(OnAnyHandler handler)
        {
            if (handler != null)
            {
                this._onAnyHandlers.Add(handler);
            }
        }

        public void PrependAny(OnAnyHandler handler)
        {
            if (handler != null)
            {
                this._onAnyHandlers.Insert(0, handler);
            }
        }

        public void OffAny(OnAnyHandler handler)
        {
            if (handler != null)
            {
                this._onAnyHandlers.Remove(handler);
            }
        }

        public OnAnyHandler[] ListenersAny() => this._onAnyHandlers.ToArray();

        internal async Task ClientAckAsync(int packetId, CancellationToken cancellationToken, params object[] data)
        {
            IMessage msg;
            if (data != null && data.Length > 0)
            {
                var result = JsonSerializer.Serialize(data);
                if (result.Bytes.Count > 0)
                {
                    msg = new ServerBinaryAckMessage
                    {
                        Id = packetId,
                        Namespace = Namespace,
                        Json = result.Json
                    };
                    msg.OutgoingBytes = new List<byte[]>(result.Bytes);
                }
                else
                {
                    msg = new ServerAckMessage
                    {
                        Namespace = Namespace,
                        Id = packetId,
                        Json = result.Json
                    };
                }
            }
            else
            {
                msg = new ServerAckMessage
                {
                    Namespace = Namespace,
                    Id = packetId
                };
            }
            await this._transport.SendAsync(msg, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Emits an event to the socket
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data">Any other parameters can be included. All serializable datastructures are supported, including byte[]</param>
        /// <returns></returns>
        public async Task EmitAsync(string eventName, params object[] data)
        {
            await EmitAsync(eventName, CancellationToken.None, data).ConfigureAwait(false);
        }

        public async Task EmitAsync(string eventName, CancellationToken cancellationToken, params object[] data)
        {
            if (data != null && data.Length > 0)
            {
                var result = JsonSerializer.Serialize(data);
                if (result.Bytes.Count > 0)
                {
                    var msg = new BinaryMessage
                    {
                        Namespace = Namespace,
                        OutgoingBytes = new List<byte[]>(result.Bytes),
                        Event = eventName,
                        Json = result.Json
                    };
                    await this._transport.SendAsync(msg, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    var msg = new EventMessage
                    {
                        Namespace = Namespace,
                        Event = eventName,
                        Json = result.Json
                    };
                    await this._transport.SendAsync(msg, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                var msg = new EventMessage
                {
                    Namespace = Namespace,
                    Event = eventName
                };
                await this._transport.SendAsync(msg, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Emits an event to the socket
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="ack">will be called with the server answer.</param>
        /// <param name="data">Any other parameters can be included. All serializable datastructures are supported, including byte[]</param>
        /// <returns></returns>
        public async Task EmitAsync(string eventName, Action<SocketIOResponse> ack, params object[] data)
        {
            await EmitAsync(eventName, CancellationToken.None, ack, data).ConfigureAwait(false);
        }

        public async Task EmitAsync(string eventName, CancellationToken cancellationToken, Action<SocketIOResponse> ack, params object[] data)
        {
            this._ackHandlers.Add(++this._packetId, ack);
            if (data != null && data.Length > 0)
            {
                var result = JsonSerializer.Serialize(data);
                if (result.Bytes.Count > 0)
                {
                    var msg = new ClientBinaryAckMessage
                    {
                        Event = eventName,
                        Namespace = Namespace,
                        Json = result.Json,
                        Id = _packetId,
                        OutgoingBytes = new List<byte[]>(result.Bytes)
                    };
                    await this._transport.SendAsync(msg, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    var msg = new ClientAckMessage
                    {
                        Event = eventName,
                        Namespace = Namespace,
                        Id = _packetId,
                        Json = result.Json
                    };
                    await this._transport.SendAsync(msg, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                var msg = new ClientAckMessage
                {
                    Event = eventName,
                    Namespace = Namespace,
                    Id = _packetId
                };
                await this._transport.SendAsync(msg, cancellationToken).ConfigureAwait(false);
            }
        }

        private async void InvokeDisconnect(string reason)
        {
            if (Connected)
            {
                Connected = false;
                OnDisconnected?.Invoke(this, reason);
                try
                {
                    await this._transport.DisconnectAsync(CancellationToken.None).ConfigureAwait(false);
                }
                catch { }
                if (reason != DisconnectReason.IOServerDisconnect && reason != DisconnectReason.IOClientDisconnect)
                {
                    //In the this cases (explicit disconnection), the client will not try to reconnect and you need to manually call socket.connect().
                    if (Options.Reconnection)
                    {
                        ConnectCore();
                    }
                }
            }
        }

        public void AddExpectedException(Type type)
        {
            if (!this._expectedExceptions.Contains(type))
            {
                this._expectedExceptions.Add(type);
            }
        }

        private void DisposeForReconnect()
        {
            this._packetId = -1;
            this._ackHandlers.Clear();
            if (this._connectionTokenSource != null)
            {
                this._connectionTokenSource.Cancel();
                this._connectionTokenSource.Dispose();
            }
        }

        public void Dispose()
        {
            //HttpClient.Dispose();
            try
            {
                this._transport.Dispose();
                this._ackHandlers.Clear();
                this._onAnyHandlers.Clear();
                this._eventHandlers.Clear();
                this._connectionTokenSource.Cancel();
                this._connectionTokenSource.Dispose();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }
}